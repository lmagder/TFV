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
using System.IO;

using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace TFV
{
    public partial class LocalTreeView : UserControl
    {
        public LocalTreeView()
        {
            InitializeComponent();

            int styleFlags = NativeMethods.TVS_EX_FADEINOUTEXPANDOS | NativeMethods.TVS_EX_DOUBLEBUFFER;
            NativeMethods.SendMessage(treeView.Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, styleFlags, styleFlags);

            NativeMethods.SetWindowTheme(treeView.Handle, "Explorer", null);

            if (DpiHelper.IsScalingRequired)
            {
                imageListOverlays.ImageSize = DpiHelper.LogicalToDeviceUnits(imageListOverlays.ImageSize);
                imageListOverlays.Images.AddStrip(DpiHelper.ScaleBitmapLogicalToDevice(TFV.Properties.Resources.TreeViewStateIcons));
            }
            else
            {
                imageListOverlays.Images.AddStrip(TFV.Properties.Resources.TreeViewStateIcons);
            }

            ImageList temp = new ImageList();
            temp.ImageSize = DpiHelper.LogicalToDeviceUnits(new Size(16,16));
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

            [DllImport("shell32.dll")]
            private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
            [StructLayout(LayoutKind.Sequential)]
            private struct SHFILEINFO
            {
                public IntPtr hIcon;
                public IntPtr iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            };
            private const uint SHGFI_ICON = 0x100;
            private const uint SHGFI_LARGEICON = 0x0; // 'Large icon
            private const uint SHGFI_SMALLICON = 0x1; // 'Small icon

            public static System.Drawing.Icon GetLargeIcon(string file)
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hImgLarge = SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
                return System.Drawing.Icon.FromHandle(shinfo.hIcon);
            }

            public static System.Drawing.Icon GetSmallIcon(string file)
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hImgLarge = SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
                return System.Drawing.Icon.FromHandle(shinfo.hIcon);
            }
        }
        private VersionControlServer m_sourceControl;
        private Workspace m_workspace;
        private WorkspaceVersionSpec m_workspaceSpec;

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


        private class TreeNodeWorking : TreeNode
        {
            public TreeNodeWorking() : base(string.Empty, 3, 3)
            {
            }
        }
        private class TreeNodeLocalItem : TreeNode
        {
            public string LocalItem { get; private set; }
            public int ItemID { get; private set; }
            public ExtendedItem ExItem { get; private set; }
            public TreeNodeLocalItem(ExtendedItem localItem, string text) : base(text, 0, 0)
            {
                this.LocalItem = localItem != null ? localItem.LocalItem : "";
                this.ItemID = localItem != null ? localItem.ItemId : 0;
                this.IsMultiSelect = false;
                this.ExItem = localItem;
            }

            public TreeNodeLocalItem(string fname, string text)
                : base(text, 0, 0)
            {
                this.LocalItem = fname;
                this.ItemID = 0;
                this.IsMultiSelect = false;
                this.ExItem = null;
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

        private SortedDictionary<string, TreeNodeLocalItem> m_selectedItems = new SortedDictionary<string, TreeNodeLocalItem>();

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
                    TreeNodeLocalItem tsi = null;
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
            if (initialPath != null && initialPath.Trim().Length > 0)
            {
                initialPath = FileSpec.GetFullPath(initialPath);

                TreeNodeLocalItem foundItem = null;
                if (TryFindNodeByServerItem(initialPath, null, out foundItem))
                {
                    foundItem.EnsureVisible();
                    m_navigateToWhenLoaded = null;
                    return;
                }

                List<string> list = new List<string>();
                m_toExpand.AddRange(initialPath.Split('\\'));
                m_navigateToWhenLoaded = initialPath;
                StartBackgroundWorkerIfNeeded();
            }
        }

        private TreeNodeLocalItem AttachTreeNode(TreeNode parent, string name, TempItemSet.TempItem item)
        {
            TreeNodeLocalItem node;
            if (parent == null)
            {
                //the root
                node = new TreeNodeLocalItem("", name);
            }
            else
            {
                if (item.ServerItem != null)
                {
                    if (item.ServerItem.ItemType == ItemType.Folder)
                    {
                        node = new TreeNodeLocalItem(item.ServerItem, name);
                        node.Nodes.Add(new TreeNodeWorking());
                    }
                    else if (item.ServerItem.ItemType == ItemType.File)
                    {
                        node = new TreeNodeLocalItem(item.ServerItem, name);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    node = new TreeNodeLocalItem(item.LocalPath, name);
                    if (Directory.Exists(item.LocalPath))
                        node.Nodes.Add(new TreeNodeWorking());
                }

                if (m_selectedItems.ContainsKey(node.LocalItem))
                {
                    node.ColorSelected();
                    m_selectedItems[node.LocalItem] = node;
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
                if (BackgroundWorkStarted != null)
                    BackgroundWorkStarted(this, EventArgs.Empty);

                backgroundWorker.RunWorkerAsync(m_toExpand.ToArray());
                m_toExpand.Clear();
            }
        }

        private bool NodeNeedsExpansion(TreeNode toCheck)
        {
            return toCheck is TreeNodeLocalItem && 1 == toCheck.Nodes.Count && toCheck.Nodes[0] is TreeNodeWorking;
        }

        private void Reset()
        {
            backgroundWorker.CancelAsync();
            m_toExpand.Clear();
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            TreeNodeLocalItem root = AttachTreeNode(null, m_workspace != null ? m_workspace.DisplayName : "wat?", new TempItemSet.TempItem());
            treeView.SelectedNode = root;
            if (m_workspace != null)
            {
                var roots = WorkingFolder.GetWorkspaceRoots(m_workspace.Folders);
                List<ItemSpec> specs = new List<ItemSpec>();
                foreach(string s in roots)
                {
                    specs.Add(new ItemSpec(s, RecursionType.None));
                }
                var items = m_workspace.GetExtendedItems(specs.ToArray(), DeletedState.Any, ItemType.Folder);
                foreach(var i in items)
                {
                    foreach(var j in i)
                    {
                        AttachTreeNode(root, j.LocalItem, new TempItemSet.TempItem { LocalPath = j.LocalItem, ServerItem = j });
                    }
                }
                    
            }
            treeView.EndUpdate();
        }

        private bool TryFindNodeByServerItem(string serverItem, TreeNodeLocalItem startNode, out TreeNodeLocalItem foundNode)
        {
            foundNode = null;
            if (treeView.Nodes.Count == 0 || !(treeView.Nodes[0] is TreeNodeLocalItem))
            {
                return false;
            }
            if (startNode == null)
            {
                TreeNodeLocalItem rootNode = (TreeNodeLocalItem)treeView.Nodes[0];
                foreach(TreeNode node in rootNode.Nodes)
                {
                    if (TryFindNodeByServerItem(serverItem, (TreeNodeLocalItem)node, out foundNode))
                        return true;
                }
                return false;
            }
            else
            {
                foundNode = startNode;
            }
            while (foundNode.LocalItem.Length < serverItem.Length)
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
                foundNode = (TreeNodeLocalItem)foundNode.Nodes[num];
            }
            return true;
        }
        private int BinarySearchForNextNode(TreeNodeLocalItem node, string serverItem)
        {
            int num = node.LocalItem.Length;
            if (serverItem[num] == '\\')
            {
                num++;
            }
            int num2 = serverItem.IndexOf('\\', num);
            int num3 = ((num2 < 0) ? serverItem.Length : num2) - num;
            int i = 0;
            int num4 = node.Nodes.Count - 1;
            while (i <= num4)
            {
                int num5 = i + (num4 - i >> 1);
                TreeNodeLocalItem treeNodeServerFolder = (TreeNodeLocalItem)node.Nodes[num5];
                int num6 = treeNodeServerFolder.LocalItem.IndexOf('\\', num);
                int num7 = ((num6 < 0) ? treeNodeServerFolder.LocalItem.Length : num6) - num;
                int num8 = string.Compare(treeNodeServerFolder.LocalItem, num, serverItem, num, Math.Min(num3, num7), StringComparison.OrdinalIgnoreCase);
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
            Navigate(curItem != null ? curItem : "");
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
            public struct TempItem
            {
                public string LocalPath;
                public ExtendedItem ServerItem;
            }
            public TempItem[] Items;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            string[] queryItems = (string[])e.Argument;
            TempItemSet[] result = new TempItemSet[queryItems.Length];
            for (int i = 0; i < queryItems.Length; i++)
            {
                result[i] = new TempItemSet();
                result[i].QueryPath = queryItems[i];

                List<ItemSpec> tempList = new List<ItemSpec>();
                List<int> tempIndx = new List<int>();
                string[] filenames = Directory.EnumerateFileSystemEntries(queryItems[i]).ToArray();
                result[i].Items = new TempItemSet.TempItem[filenames.Length];

                for (int j = 0; j < filenames.Length; j++)
                {
                    result[i].Items[j].LocalPath = filenames[j];
                    try
                    {
                        string tfsPath = m_workspace.GetServerItemForLocalItem(filenames[j]);
                        tempList.Add(new ItemSpec(tfsPath, RecursionType.None));
                        tempIndx.Add(j);
                    }
                    catch (ItemNotMappedException) { }
                    catch (ItemCloakedException) { }
                }
                ExtendedItem[][] itemSets = m_workspace.GetExtendedItems(tempList.ToArray(), DeletedState.Any, ItemType.Any);
                for (int j = 0; j < itemSets.Length; j++)
                {
                    if (itemSets[j].Length > 0)
                    {
                        result[i].Items[tempIndx[j]].ServerItem = itemSets[j][0];
                    }
                }
                Array.Sort(result[i].Items, (a, b) => a.LocalPath.CompareTo(b.LocalPath));
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
                TreeNodeLocalItem treeNodeServerFolder = null;
                foreach (TempItemSet itemSet in itemSets)
                {
                    if (itemSet.QueryPath != null)
                    {
                        if (treeNodeServerFolder != null && !FileSpec.IsSubItem(itemSet.QueryPath, treeNodeServerFolder.LocalItem))
                        {
                            treeNodeServerFolder = null;
                        }
                        TreeNodeLocalItem treeNodeServerFolder2;
                        if (TryFindNodeByServerItem(itemSet.QueryPath, treeNodeServerFolder, out treeNodeServerFolder2) && NodeNeedsExpansion(treeNodeServerFolder2))
                        {
                            treeNodeServerFolder2.Nodes.Clear();
                            foreach (TempItemSet.TempItem i in itemSet.Items)
                            {
                                AttachTreeNode(treeNodeServerFolder2, Path.GetFileName(i.LocalPath), i);
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
                    TreeNodeLocalItem treeNodeServerFolder3;
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

        private void SetExistingNodeSelection(TreeNodeLocalItem treeNodeServerFolder, bool toggle)
        {
            if (m_selectedItems.ContainsKey(treeNodeServerFolder.LocalItem))
            {
                if (toggle)
                {
                    treeNodeServerFolder.ColorUnselected();
                    m_selectedItems.Remove(treeNodeServerFolder.LocalItem);
                }
            }
            else
            {
                treeNodeServerFolder.ColorSelected();
                m_selectedItems.Add(treeNodeServerFolder.LocalItem, treeNodeServerFolder);
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
            if (e.Node is TreeNodeLocalItem)
            {
                TreeNodeLocalItem treeNodeServerFolder = (TreeNodeLocalItem)e.Node;

                treeView.BeginUpdate();
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    SetExistingNodeSelection(treeNodeServerFolder, ModifierKeys.HasFlag(Keys.Control));
                    TreeNodeLocalItem prevItem = treeNodeServerFolder.PrevVisibleNode as TreeNodeLocalItem;
                    while (prevItem != null && !prevItem.IsMultiSelect)
                    {
                        SetExistingNodeSelection(prevItem, ModifierKeys.HasFlag(Keys.Control));
                        prevItem = prevItem.PrevVisibleNode as TreeNodeLocalItem;
                    }
                }
                else
                {
                    if (!ModifierKeys.HasFlag(Keys.Control))
                    {
                        ClearExistingNodeSelection(new string[] { treeNodeServerFolder.LocalItem });
                    }
                    SetExistingNodeSelection(treeNodeServerFolder, ModifierKeys.HasFlag(Keys.Control));
                }
                treeView.EndUpdate();
                e.Cancel = true;
                this.m_lastSelectedPath = treeNodeServerFolder.LocalItem;
                if (LastSelectedServerItemChanged != null)
                    LastSelectedServerItemChanged(this, new EventArgs());
            }
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            TreeNodeLocalItem titem = e.Node as TreeNodeLocalItem;

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
                var imageSize = treeView.ImageList.ImageSize;
                drawLoc.X -= imageSize.Width;
                using (SolidBrush b = new SolidBrush(titem.BackColor))
                {
                    e.Graphics.FillRectangle(b, drawLoc.X, drawLoc.Y, titem.Bounds.Width + imageSize.Width, titem.Bounds.Height);
                }
                Rectangle textRect = vsr.GetBackgroundContentRectangle(e.Graphics, titem.Bounds);

                var pixelSize = DpiHelper.LogicalToDeviceUnits(new Size(1, 1));
                textRect.Offset(pixelSize.Width, pixelSize.Height);
                bool disabled = titem.ExItem != null ? !titem.ExItem.IsInWorkspace : false;
                vsr.DrawText(e.Graphics, textRect, titem.Text, disabled, TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);

                ExtendedItem item = titem.ExItem;

                Icon fileIcon = string.IsNullOrEmpty(titem.LocalItem) ? TFV.Properties.Resources.TFSServer : NativeMethods.GetSmallIcon(titem.LocalItem);
                e.Graphics.DrawIcon(fileIcon, new Rectangle(drawLoc, imageSize));
               
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
            if (e.Node is TreeNodeLocalItem)
            {
                TreeNodeLocalItem treeNodeServerFolder = (TreeNodeLocalItem)e.Node;
                if (NodeNeedsExpansion(e.Node))
                {
                    m_toExpand.Add(treeNodeServerFolder.LocalItem);
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
