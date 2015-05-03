using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace TFV
{
    public partial class ServerTreeView : UserControl
    {
        public ServerTreeView()
        {
            InitializeComponent();

            int styleFlags = NativeMethods.TVS_EX_FADEINOUTEXPANDOS | NativeMethods.TVS_EX_DOUBLEBUFFER;
            NativeMethods.SendMessage(treeView.Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, styleFlags, styleFlags);

            NativeMethods.SetWindowTheme(treeView.Handle, "Explorer", null);

            if (DpiHelper.IsScalingRequired)
            {
                imageListIcons.ImageSize = DpiHelper.LogicalToDeviceUnits(imageListIcons.ImageSize);
                imageListOverlays.ImageSize = DpiHelper.LogicalToDeviceUnits(imageListOverlays.ImageSize);
                imageListIcons.Images.AddStrip(DpiHelper.ScaleBitmapLogicalToDevice(TFV.Properties.Resources.TreeViewIcons));
                imageListOverlays.Images.AddStrip(DpiHelper.ScaleBitmapLogicalToDevice(TFV.Properties.Resources.TreeViewStateIcons));
            }
            else
            {
                imageListIcons.Images.AddStrip(TFV.Properties.Resources.TreeViewIcons);
                imageListOverlays.Images.AddStrip(TFV.Properties.Resources.TreeViewStateIcons);
            }

            ImageList temp = new ImageList();
            temp.ImageSize = imageListIcons.ImageSize;
            temp.ColorDepth = ColorDepth.Depth32Bit;
            treeView.ImageList = temp;
        }


        internal class NativeMethods
        {
            public const int TV_FIRST = 0x1100;
            public const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
            public const int TVM_GETEXTENDEDSTYLE = TV_FIRST + 45;
            public const int TVS_EX_FADEINOUTEXPANDOS = 0x0040;
            public const int TVS_EX_DOUBLEBUFFER = 0x0004;


            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

            [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
            public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        }
        private VersionControlServer m_sourceControl;
        private Workspace m_workspace;
        private WorkspaceVersionSpec m_workspaceSpec;
        private bool m_limitToWorkspace;
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

        public Workspace Workspace
        {
            get { return m_workspace; }
            set
            {
                if (value != m_workspace)
                {
                    m_workspace = value;
                    m_workspaceSpec = new WorkspaceVersionSpec(m_workspace);
                    Reload();
                }
            }
        }

        public bool LimitToWorkspace
        {
            get { return m_limitToWorkspace; }
            set
            {
                if (value != m_limitToWorkspace)
                {
                    m_limitToWorkspace = value;
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
            public int CollapsedImageIndex { get; private set; }
            public int ExpandedImageIndex { get; private set; }
            public int ItemID { get; private set; }
            public ExtendedItem ExItem { get; private set; }
            public TreeNodeServerItem(ExtendedItem serverItem, string text, int imageIndex, int expandedImageIndex) : base(text, 0, 0)
            {
                this.ServerItem = serverItem != null ? serverItem.TargetServerItem : "$/";
                this.CollapsedImageIndex = imageIndex;
                this.ExpandedImageIndex = expandedImageIndex;
                this.ItemID = serverItem != null ? serverItem.ItemId : 0;
                this.IsMultiSelect = false;
                this.ExItem = serverItem;
            }

            public bool IsMultiSelect { get; private set; }
            public void ColorSelected()
            {
                BackColor = SystemColors.Highlight;
                IsMultiSelect = true;
            }

            public void ColorUnselected()
            {
                BackColor = Color.Empty;
                IsMultiSelect = false;
            }
        }

        private List<string> m_toExpand = new List<string>();
        private string m_lastSelectedPath, m_navigateToWhenLoaded;

        private SortedDictionary<string, TreeNodeServerItem> m_selectedItems = new SortedDictionary<string, TreeNodeServerItem>();

        public event EventHandler LastSelectedServerItemChanged;
        public event EventHandler BackgroundWorkStarted;
        public event EventHandler BackgroundWorkEnded;

        public string LastSelectedServerItem
        {
            get
            {
                return this.m_lastSelectedPath ?? string.Empty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<string> SelectedServerItems
        {
            get
            {
                return m_selectedItems.Keys;
            }
            set
            {
                treeView.BeginUpdate();
                ClearExistingNodeSelection(value);
                foreach (var i in value)
                {
                    TreeNodeServerItem tsi = null;
                    TryFindNodeByServerItem(i, null, out tsi);
                    if (tsi != null)
                    {
                        tsi.ColorSelected();
                    }
                    m_selectedItems.Add(i, tsi);
                }
                treeView.EndUpdate();
            }
        }

        public void Navigate(string initialPath)
        {
            if (initialPath != null && initialPath.StartsWith("$/") && initialPath.Length > 2)
            {
                initialPath = VersionControlPath.GetFullPath(initialPath);

                TreeNodeServerItem foundItem = null;
                if (TryFindNodeByServerItem(initialPath, null, out foundItem))
                {
                    foundItem.EnsureVisible();
                    m_navigateToWhenLoaded = null;
                    return;
                }

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

        private TreeNodeServerItem AttachTreeNode(TreeNode parent, ExtendedItem item)
        {
            TreeNodeServerItem node;
            if (item == null)
            {
                //the root
                node = new TreeNodeServerItem(item, m_sourceControl.TeamProjectCollection.Name, 6, 6);
                node.Nodes.Add(new TreeNodeWorking());
            }
            else
            {
                string fileName = VersionControlPath.GetFileName(item.TargetServerItem);
                if (item.IsBranch)
                {
                    if (item.DeletionId != 0)
                    {
                        node = new TreeNodeServerItem(item, fileName, 2, 2);
                    }
                    else
                    {
                        node = new TreeNodeServerItem(item, fileName, 1, 1);
                    }
                    node.Nodes.Add(new TreeNodeWorking());
                }
                else if (item.ItemType == ItemType.Folder)
                {
                    if (VersionControlPath.GetFolderDepth(item.TargetServerItem) == 1)
                    {
                        if (item.DeletionId != 0)
                        {
                            node = new TreeNodeServerItem(item, fileName, 3, 3);
                        }
                        else
                        {
                            node = new TreeNodeServerItem(item, fileName, 7, 7);
                        }
                    }
                    else
                    {
                        if (item.DeletionId != 0)
                        {
                            node = new TreeNodeServerItem(item, fileName, 3, 3);
                        }
                        else
                        {
                            node = new TreeNodeServerItem(item, fileName, 4, 5);
                        }
                    }
                    node.Nodes.Add(new TreeNodeWorking());
                }
                else if (item.ItemType == ItemType.File)
                {
                    if (item.DeletionId != 0)
                    {
                        node = new TreeNodeServerItem(item, fileName, 3, 3);
                    }
                    else
                    {
                        node = new TreeNodeServerItem(item, fileName, 12, 12);
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

            if (m_selectedItems.ContainsKey(node.ServerItem))
            {
                node.ColorSelected();
                m_selectedItems[node.ServerItem] = node;
            }



            return node;

        }

        private void StartBackgroundWorkerIfNeeded()
        {
            if (this.m_toExpand.Count > 0 && !backgroundWorker.IsBusy)
            {
                if (BackgroundWorkStarted != null)
                    BackgroundWorkStarted(this, EventArgs.Empty);

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
            string curItem = LastSelectedServerItem;
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


        class TempItemSet
        {
            public string QueryPath;
            public ExtendedItem[] Items;
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
            ExtendedItem[][] itemSets;
            itemSets = m_workspace.GetExtendedItems(array, m_deletedState, ShowFiles ? ItemType.Any : ItemType.Folder, GetItemsOptions.IncludeBranchInfo);
            TempItemSet[] result = new TempItemSet[itemSets.Length];
            for (int i = 0; i < itemSets.Length; i++)
            {
                result[i] = new TempItemSet
                {
                    QueryPath = VersionControlPath.GetFolderName(array[i].Item),
                    Items = itemSets[i].Where((item) => (item.TargetServerItem != queryItems[i] && (item.IsInWorkspace || !m_limitToWorkspace))).ToArray()
                };
            }

            if (m_limitToWorkspace)
            {
                Workspace ws = m_workspace;
                ws.RefreshIfNeeded();
                var folders = ws.Folders;
                foreach (TempItemSet its in result)
                {
                    int desiredCloakDepth = VersionControlPath.GetFolderDepth(its.QueryPath) + 1;
                    string cloakedWeSaw = null;
                    List<ItemSpec> newItems = new List<ItemSpec>();
                    foreach (var fldr in folders)
                    {
                        if (fldr.IsCloaked && fldr.Depth != RecursionType.None && VersionControlPath.IsSubItem(fldr.ServerItem, its.QueryPath) &&
                            VersionControlPath.GetFolderDepth(fldr.ServerItem) == desiredCloakDepth)
                        {
                            cloakedWeSaw = fldr.ServerItem;
                        }
                        else if (!fldr.IsCloaked && cloakedWeSaw != null && VersionControlPath.IsSubItem(fldr.ServerItem, cloakedWeSaw))
                        {
                            //we need this to keep going even if it's cloaked
                            newItems.Add(new ItemSpec(cloakedWeSaw, RecursionType.None));
                            cloakedWeSaw = null;
                        }
                    }
                    if (newItems.Count > 0)
                    {
                        ExtendedItem[] newExItems = new ExtendedItem[its.Items.Length + newItems.Count];
                        Array.Copy(its.Items, newExItems, its.Items.Length);
                        ExtendedItem[][] tempExItems = m_sourceControl.GetExtendedItems(newItems.ToArray(), m_deletedState, ItemType.Folder, GetItemsOptions.IncludeBranchInfo);
                        for (int i = 0; i < tempExItems.Length; i++)
                            newExItems[i + its.Items.Length] = tempExItems[i][0];

                        Array.Sort(newExItems, (a, b) => a.TargetServerItem.CompareTo(b.TargetServerItem));
                        its.Items = newExItems;
                    }
                }
            }

            e.Result = result;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BackgroundWorkEnded != null)
                BackgroundWorkEnded(this, EventArgs.Empty);

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
                            foreach (ExtendedItem i in itemSet.Items)
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

        private void SetExistingNodeSelection(TreeNodeServerItem treeNodeServerFolder, bool toggle)
        {
            if (m_selectedItems.ContainsKey(treeNodeServerFolder.ServerItem))
            {
                if (toggle)
                {
                    treeNodeServerFolder.ColorUnselected();
                    m_selectedItems.Remove(treeNodeServerFolder.ServerItem);
                }
            }
            else
            {
                treeNodeServerFolder.ColorSelected();
                m_selectedItems.Add(treeNodeServerFolder.ServerItem, treeNodeServerFolder);
            }
        }

        private void ClearExistingNodeSelection(ICollection<string> leaveAlone = null)
        {
            foreach (var node in m_selectedItems)
            {
                if (node.Value != null)
                {
                    if (leaveAlone != null && leaveAlone.Contains(node.Key))
                        continue;
                    node.Value.ColorUnselected();
                }
            }

            m_selectedItems.Clear();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is TreeNodeServerItem)
            {
                TreeNodeServerItem treeNodeServerFolder = (TreeNodeServerItem)e.Node;

                treeView.BeginUpdate();
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    SetExistingNodeSelection(treeNodeServerFolder, ModifierKeys.HasFlag(Keys.Control));
                    TreeNodeServerItem prevItem = treeNodeServerFolder.PrevVisibleNode as TreeNodeServerItem;
                    while (prevItem != null && !prevItem.IsMultiSelect)
                    {
                        SetExistingNodeSelection(prevItem, ModifierKeys.HasFlag(Keys.Control));
                        prevItem = prevItem.PrevVisibleNode as TreeNodeServerItem;
                    }
                }
                else
                {
                    if (!ModifierKeys.HasFlag(Keys.Control))
                    {
                        ClearExistingNodeSelection(new string[] { treeNodeServerFolder.ServerItem });
                    }
                    SetExistingNodeSelection(treeNodeServerFolder, ModifierKeys.HasFlag(Keys.Control));
                }
                treeView.EndUpdate();
                e.Cancel = true;
                this.m_lastSelectedPath = treeNodeServerFolder.ServerItem;
                if (LastSelectedServerItemChanged != null)
                    LastSelectedServerItemChanged(this, new EventArgs());
            }
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            TreeNodeServerItem titem = e.Node as TreeNodeServerItem;
            if (titem != null)
            {

                VisualStyleElement element = VisualStyleElement.TreeView.Item.Normal;
                if (e.State.HasFlag(TreeNodeStates.Selected) || titem.IsMultiSelect)
                {
                    element = e.Node.TreeView.Focused ? VisualStyleElement.TreeView.Item.Selected : VisualStyleElement.TreeView.Item.SelectedNotFocus;
                }
                else if (e.State.HasFlag(TreeNodeStates.Hot))
                {
                    element = VisualStyleElement.TreeView.Item.Hot;
                }

                VisualStyleRenderer vsr = new VisualStyleRenderer("Explorer::TreeView", element.Part, element.State);
                Point drawLoc = titem.Bounds.Location;
                drawLoc.X -= imageListIcons.ImageSize.Width;
                using (SolidBrush b = new SolidBrush(titem.BackColor))
                {
                    e.Graphics.FillRectangle(b, drawLoc.X, drawLoc.Y, titem.Bounds.Width + imageListIcons.ImageSize.Width, titem.Bounds.Height);
                }
                Rectangle textRect = vsr.GetBackgroundContentRectangle(e.Graphics, titem.Bounds);

                var pixelSize = DpiHelper.LogicalToDeviceUnits(new Size(1, 1));
                textRect.Offset(pixelSize.Width, pixelSize.Height);
                bool disabled = titem.ExItem != null ? !titem.ExItem.IsInWorkspace : false;
                vsr.DrawText(e.Graphics, textRect, titem.Text, disabled, TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);

                imageListIcons.Draw(e.Graphics, drawLoc, titem.IsExpanded ? titem.ExpandedImageIndex : titem.CollapsedImageIndex);
                ExtendedItem item = titem.ExItem;
                if (item != null)
                {

                    if (item.ChangeType.HasFlag(ChangeType.Add))
                        imageListOverlays.Draw(e.Graphics, drawLoc, 0);
                    else if (item.ChangeType.HasFlag(ChangeType.Delete))
                        imageListOverlays.Draw(e.Graphics, drawLoc, 3);
                    else if (item.ChangeType.HasFlag(ChangeType.Rollback))
                        imageListOverlays.Draw(e.Graphics, drawLoc, 8);
                    else if (item.ChangeType.HasFlag(ChangeType.Merge))
                        imageListOverlays.Draw(e.Graphics, drawLoc, 5);
                    else if (item.ChangeType.HasFlag(ChangeType.Edit))
                        imageListOverlays.Draw(e.Graphics, drawLoc, 2);

                    if (!item.IsLatest && item.IsInWorkspace)
                    {
                        imageListOverlays.Draw(e.Graphics, drawLoc, 1);
                    }
                    if (item.HasOtherPendingChange)
                    {
                        imageListOverlays.Draw(e.Graphics, drawLoc, 7);
                    }
                }
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

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (!NodeNeedsExpansion(e.Node) && e.Node.Nodes.Count > 0)
            {
                e.Node.Nodes.Clear();
                e.Node.Nodes.Add(new TreeNodeWorking());
            }
        }
    }
}
