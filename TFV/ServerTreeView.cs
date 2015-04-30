using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace TFV
{
    public partial class ServerTreeView : UserControl
    {
        

        private VersionControlServer m_sourceControl;
        private VersionSpec m_versionSpec = VersionSpec.Latest;
		private DeletedState m_deletedState;

        public VersionControlServer SourceControl
        {
            get { return m_sourceControl; }
            set
            {
                if (value != SourceControl)
                {
                    m_sourceControl = value;
                    Reload();
                }
            }
        }

        public VersionSpec VersionSpec
        {
            get { return m_versionSpec; }
            set
            {
                if (value != m_versionSpec)
                {
                    m_versionSpec = value;
                    Reload();
                }
            }
        }

        public DeletedState DeletedState
        {
            get { return m_deletedState; }
            set
            {
                if (value != m_deletedState)
                {
                    m_deletedState = value;
                    Reload();
                }
            }
        }


		
		private class TreeNodeWorking : TreeNode
		{
			public TreeNodeWorking() : base(string.Empty, 3, 3)
			{
			}
		}
		private class TreeNodeServerItem : TreeNode
		{
			public string ServerItem { get; private set; }
			public int CollapsedImageIndex  { get; private set; }
			public int ExpandedImageIndex  { get; private set; }
            public int ItemID { get; private set; }
			public TreeNodeServerItem(Item serverItem, string text, int imageIndex, int expandedImageIndex) : base(text, imageIndex, imageIndex)
			{
				this.ServerItem = serverItem != null ? serverItem.ServerItem : "$/";
				this.CollapsedImageIndex = imageIndex;
				this.ExpandedImageIndex = expandedImageIndex;
                this.ItemID = serverItem != null ? serverItem.ItemId : 0;
			}
		}

        private List<string> m_toExpand = new List<string>();
		private string m_currentPath, m_navigateToWhenLoaded;
		public event EventHandler CurrentServerItemChanged;
		public string CurrentServerItem
		{
			get
			{
				return this.m_currentPath ?? string.Empty;
			}
			set
			{
				this.Navigate(value);
			}
		}

		
		public void Navigate(string initialPath)
		{
			if (initialPath != null && initialPath.StartsWith("$/") && initialPath.Length > 2)
			{
				initialPath = VersionControlPath.GetFullPath(initialPath);
				List<string> list = new List<string>();
				string folderName = VersionControlPath.GetFolderName(initialPath);
				while (!VersionControlPath.Equals(folderName, "$/"))
				{
					list.Add(folderName);
					folderName = VersionControlPath.GetFolderName(folderName);
				}
				list.Add("$/");
				list.Reverse();
                m_toExpand.AddRange(list);
                m_navigateToWhenLoaded = initialPath;
				StartBackgroundWorkerIfNeeded();
			}
		}

        private TreeNodeServerItem AttachTreeNode(TreeNode parent, Item item)
		{
            TreeNodeServerItem node;
            if (item == null)
            {
                //the root
                node = new TreeNodeServerItem(item, m_sourceControl.TeamProjectCollection.Name, 5, 5);
                node.Nodes.Add(new TreeNodeWorking());
            }
            else
            {
                string fileName = VersionControlPath.GetFileName(item.ServerItem);
                if (item.IsBranch)
                {
                    if (item.DeletionId != 0)
                    {
                        node = new TreeNodeServerItem(item, fileName, 1, 1);
                    }
                    else
                    {
                        node = new TreeNodeServerItem(item, fileName, 0, 0);
                    }
                    node.Nodes.Add(new TreeNodeWorking());
                }
                else if (item.ItemType == ItemType.Folder)
                {
                    if (VersionControlPath.GetFolderDepth(item.ServerItem) == 1)
                    {
                        if (item.DeletionId != 0)
                        {
                            node = new TreeNodeServerItem(item, fileName, 2, 2);
                        }
                        else
                        {
                            node = new TreeNodeServerItem(item, fileName, 6, 6);
                        }
                    }
                    else
                    {
                        if (item.DeletionId != 0)
                        {
                            node = new TreeNodeServerItem(item, fileName, 2, 2);
                        }
                        else
                        {
                            node = new TreeNodeServerItem(item, fileName, 3, 4);
                        }
                    }
                    node.Nodes.Add(new TreeNodeWorking());
                }
                else if (item.ItemType == ItemType.File)
                {
                    if (item.DeletionId != 0)
                    {
                        node = new TreeNodeServerItem(item, fileName, 2, 2);
                    }
                    else
                    {
                        node = new TreeNodeServerItem(item, fileName, 11, 11);
                    }
                }
                else
                {
                    return null;
                }
            }

            if (parent != null)
            {
                parent.Nodes.Add(node);
            }
            else
            {
                treeView.Nodes.Add(node);
            }
            return node;

		}
	
		private void StartBackgroundWorkerIfNeeded()
		{
			if (this.m_toExpand.Count > 0 && !backgroundWorker.IsBusy)
			{
                backgroundWorker.RunWorkerAsync(m_toExpand.ToArray());
				m_toExpand.Clear();
			}
		}

		private bool NodeNeedsExpansion(TreeNode toCheck)
		{
			return toCheck is TreeNodeServerItem && 1 == toCheck.Nodes.Count && toCheck.Nodes[0] is TreeNodeWorking;
		}

		private void Reset()
		{
            backgroundWorker.CancelAsync();
			m_toExpand.Clear();
			treeView.BeginUpdate();
            treeView.Nodes.Clear();
            treeView.SelectedNode = this.AttachTreeNode(null, null);
            treeView.EndUpdate();
		}

        private bool TryFindNodeByServerItem(string serverItem, TreeNodeServerItem startNode, out TreeNodeServerItem foundNode)
		{
			foundNode = null;
            if (treeView.Nodes.Count == 0 || !(treeView.Nodes[0] is TreeNodeServerItem))
			{
				return false;
			}
			if (startNode == null)
			{
                foundNode = (TreeNodeServerItem)treeView.Nodes[0];
			}
			else
			{
				foundNode = startNode;
			}
			while (foundNode.ServerItem.Length < serverItem.Length)
			{
				if (this.NodeNeedsExpansion(foundNode))
				{
					return false;
				}
				int num = BinarySearchForNextNode(foundNode, serverItem);
				if (num < 0)
				{
					return false;
				}
                foundNode = (TreeNodeServerItem)foundNode.Nodes[num];
			}
			return true;
		}
        private int BinarySearchForNextNode(TreeNodeServerItem node, string serverItem)
		{
			int num = node.ServerItem.Length;
			if (serverItem[num] == '/')
			{
				num++;
			}
			int num2 = serverItem.IndexOf('/', num);
			int num3 = ((num2 < 0) ? serverItem.Length : num2) - num;
			int i = 0;
			int num4 = node.Nodes.Count - 1;
			while (i <= num4)
			{
				int num5 = i + (num4 - i >> 1);
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)node.Nodes[num5];
				int num6 = treeNodeServerFolder.ServerItem.IndexOf('/', num);
				int num7 = ((num6 < 0) ? treeNodeServerFolder.ServerItem.Length : num6) - num;
				int num8 = string.Compare(treeNodeServerFolder.ServerItem, num, serverItem, num, Math.Min(num3, num7), StringComparison.OrdinalIgnoreCase);
				if (num8 == 0)
				{
					if (num3 == num7)
					{
						return num5;
					}
					num8 = num7 - num3;
				}
				if (num8 < 0)
				{
					i = num5 + 1;
				}
				else
				{
					num4 = num5 - 1;
				}
			}
			return ~i;
		}

        private bool m_showFiles = true;

        private void Reload()
        {
            string curItem = CurrentServerItem;
            Reset();
            Navigate(curItem != null ? curItem : "$/");
        }

        public bool ShowFiles 
        {
            get { return m_showFiles; }
            set
            {
                if (value != m_showFiles)
                {
                    m_showFiles = value;
                    Reload();
                }
            }
        }

        public ServerTreeView()
        {
            InitializeComponent();
            imageListIcons.Images.AddStrip(TFV.Properties.Resources.TreeViewIcons);
            imageListOverlays.Images.AddStrip(TFV.Properties.Resources.TreeViewStateIcons);
        }

        class TempItemSet
        {
            public string QueryPath;
            public Item[] Items;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            string[] queryItems = (string[])e.Argument;
            ItemSpec[] array = new ItemSpec[queryItems.Length];
            for (int i = 0; i < queryItems.Length; i++)
            {
                array[i] = new ItemSpec(VersionControlPath.Combine(queryItems[i], "*"), RecursionType.OneLevel);
            }
            ItemSet[] itemSets = m_sourceControl.GetItems(array, this.m_versionSpec, this.m_deletedState, ShowFiles ? ItemType.Any : ItemType.Folder, GetItemsOptions.IncludeBranchInfo);
            TempItemSet[] result = new TempItemSet[itemSets.Length];
            for (int i = 0; i < itemSets.Length; i++)
                result[i] = new TempItemSet{ QueryPath = itemSets[i].QueryPath, Items = itemSets[i].Items};

            if (m_versionSpec is WorkspaceVersionSpec)
            {
                WorkspaceVersionSpec wsvs = m_versionSpec as WorkspaceVersionSpec;
                Workspace ws = m_sourceControl.GetWorkspace(wsvs.Name, wsvs.OwnerName);
                ws.RefreshIfNeeded();
                var folders = ws.Folders;
                foreach (TempItemSet its in result)
                {
                    int desiredCloakDepth = VersionControlPath.GetFolderDepth(its.QueryPath) + 1;
                    string cloakedWeSaw = null;
                    List<Item> newItems = new List<Item>();
                    foreach (var fldr in folders)
                    {
                        if (fldr.IsCloaked &&  fldr.Depth != RecursionType.None && VersionControlPath.IsSubItem(fldr.ServerItem, its.QueryPath) &&
                            VersionControlPath.GetFolderDepth(fldr.ServerItem) == desiredCloakDepth)
                        {
                            cloakedWeSaw = fldr.ServerItem;
                        }
                        else if (!fldr.IsCloaked && cloakedWeSaw != null && VersionControlPath.IsSubItem(fldr.ServerItem, cloakedWeSaw))
                        {
                            //we need this to keep going even if it's cloaked
                            newItems.Add(m_sourceControl.GetItem(cloakedWeSaw));
                            cloakedWeSaw = null;
                        }
                    }
                    if (newItems.Count > 0)
                    {
                        newItems.AddRange(its.Items);
                        its.Items = newItems.ToArray();
                        Array.Sort(its.Items, Item.Comparer);
                       
                    }
                }
            }
            e.Result = result;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;

            TempItemSet[] itemSets = e.Result as TempItemSet[];
            if (itemSets != null)
            {
                treeView.BeginUpdate();
                TreeNodeServerItem treeNodeServerFolder = null;
                foreach (TempItemSet itemSet in itemSets)
                {
                    if (itemSet.QueryPath != null)
                    {
                        if (treeNodeServerFolder != null && !VersionControlPath.IsSubItem(itemSet.QueryPath, treeNodeServerFolder.ServerItem))
                        {
                            treeNodeServerFolder = null;
                        }
                        TreeNodeServerItem treeNodeServerFolder2;
                        if (TryFindNodeByServerItem(itemSet.QueryPath, treeNodeServerFolder, out treeNodeServerFolder2) && NodeNeedsExpansion(treeNodeServerFolder2))
                        {
                            treeNodeServerFolder2.Nodes.Clear();
                            foreach(Item i in itemSet.Items)
                            {
                                AttachTreeNode(treeNodeServerFolder2, i);
                            }
                            if (!treeNodeServerFolder2.IsExpanded && m_navigateToWhenLoaded == null)
                            {
                                treeNodeServerFolder2.Expand();
                            }
                            treeNodeServerFolder = treeNodeServerFolder2;
                        }
                    }
                }
                if (m_navigateToWhenLoaded != null)
                {
                    TreeNodeServerItem treeNodeServerFolder3;
                    TryFindNodeByServerItem(m_navigateToWhenLoaded, null, out treeNodeServerFolder3);
                    if (treeNodeServerFolder3 != null)
                    {
                        treeNodeServerFolder3.EnsureVisible();
                        treeView.SelectedNode = treeNodeServerFolder3;
                    }
                    m_navigateToWhenLoaded = null;
                }
                treeView.EndUpdate();
            }
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TreeNodeServerItem)
            {
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)e.Node;
                if (treeNodeServerFolder.ImageIndex != treeNodeServerFolder.CollapsedImageIndex)
                {
                    treeNodeServerFolder.ImageIndex = treeNodeServerFolder.CollapsedImageIndex;
                    treeNodeServerFolder.SelectedImageIndex = treeNodeServerFolder.CollapsedImageIndex;
                }
            }
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TreeNodeServerItem)
            {
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)e.Node;
                if (treeNodeServerFolder.ImageIndex != treeNodeServerFolder.ExpandedImageIndex)
                {
                    treeNodeServerFolder.ImageIndex = treeNodeServerFolder.ExpandedImageIndex;
                    treeNodeServerFolder.SelectedImageIndex = treeNodeServerFolder.ExpandedImageIndex;
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TreeNodeServerItem)
            {
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)e.Node;
                this.m_currentPath = treeNodeServerFolder.ServerItem;
                if (CurrentServerItemChanged != null)
                    CurrentServerItemChanged(this, new EventArgs());
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is TreeNodeServerItem)
            {
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)e.Node;
                if (NodeNeedsExpansion(e.Node))
                {
                    m_toExpand.Add(treeNodeServerFolder.ServerItem);
                    StartBackgroundWorkerIfNeeded();
                    e.Cancel = true;
                }
            }
        }
    }
}
