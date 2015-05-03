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
        Icon m_fileIcon, m_addedFileIcon, m_folderOpen, m_folderClosed;
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

            m_fileIcon = NativeMethods.GetSmallIcon(NativeMethods.SHSTOCKICONID.SIID_DOCNOASSOC);
            m_addedFileIcon = NativeMethods.GetSmallIcon(NativeMethods.SHSTOCKICONID.SIID_DOCASSOC);
            m_folderOpen = NativeMethods.GetSmallIcon(NativeMethods.SHSTOCKICONID.SIID_FOLDEROPEN);
            m_folderClosed = NativeMethods.GetSmallIcon(NativeMethods.SHSTOCKICONID.SIID_FOLDERBACK);
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

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool DestroyIcon(IntPtr hIcon);

            private const uint SHGFI_ICON = 0x100;
            private const uint SHGFI_LARGEICON = 0x0; // 'Large icon
            private const uint SHGFI_SMALLICON = 0x1; // 'Small icon

            public static System.Drawing.Icon GetLargeIcon(string file)
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hImgLarge = SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
                Icon ret = Icon.FromHandle(shinfo.hIcon);
                //DestroyIcon(shinfo.hIcon);
                return ret;
            }

            public static System.Drawing.Icon GetSmallIcon(string file)
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hImgLarge = SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
                Icon ret = Icon.FromHandle(shinfo.hIcon);
                //DestroyIcon(shinfo.hIcon);
                return ret;
            }

            public enum SHSTOCKICONID : uint
            {
                SIID_DOCNOASSOC = 0,
                SIID_DOCASSOC = 1,
                SIID_APPLICATION = 2,
                SIID_FOLDER = 3,
                SIID_FOLDEROPEN = 4,
                SIID_DRIVE525 = 5,
                SIID_DRIVE35 = 6,
                SIID_DRIVEREMOVE = 7,
                SIID_DRIVEFIXED = 8,
                SIID_DRIVENET = 9,
                SIID_DRIVENETDISABLED = 10,
                SIID_DRIVECD = 11,
                SIID_DRIVERAM = 12,
                SIID_WORLD = 13,
                SIID_SERVER = 15,
                SIID_PRINTER = 16,
                SIID_MYNETWORK = 17,
                SIID_FIND = 22,
                SIID_HELP = 23,
                SIID_SHARE = 28,
                SIID_LINK = 29,
                SIID_SLOWFILE = 30,
                SIID_RECYCLER = 31,
                SIID_RECYCLERFULL = 32,
                SIID_MEDIACDAUDIO = 40,
                SIID_LOCK = 47,
                SIID_AUTOLIST = 49,
                SIID_PRINTERNET = 50,
                SIID_SERVERSHARE = 51,
                SIID_PRINTERFAX = 52,
                SIID_PRINTERFAXNET = 53,
                SIID_PRINTERFILE = 54,
                SIID_STACK = 55,
                SIID_MEDIASVCD = 56,
                SIID_STUFFEDFOLDER = 57,
                SIID_DRIVEUNKNOWN = 58,
                SIID_DRIVEDVD = 59,
                SIID_MEDIADVD = 60,
                SIID_MEDIADVDRAM = 61,
                SIID_MEDIADVDRW = 62,
                SIID_MEDIADVDR = 63,
                SIID_MEDIADVDROM = 64,
                SIID_MEDIACDAUDIOPLUS = 65,
                SIID_MEDIACDRW = 66,
                SIID_MEDIACDR = 67,
                SIID_MEDIACDBURN = 68,
                SIID_MEDIABLANKCD = 69,
                SIID_MEDIACDROM = 70,
                SIID_AUDIOFILES = 71,
                SIID_IMAGEFILES = 72,
                SIID_VIDEOFILES = 73,
                SIID_MIXEDFILES = 74,
                SIID_FOLDERBACK = 75,
                SIID_FOLDERFRONT = 76,
                SIID_SHIELD = 77,
                SIID_WARNING = 78,
                SIID_INFO = 79,
                SIID_ERROR = 80,
                SIID_KEY = 81,
                SIID_SOFTWARE = 82,
                SIID_RENAME = 83,
                SIID_DELETE = 84,
                SIID_MEDIAAUDIODVD = 85,
                SIID_MEDIAMOVIEDVD = 86,
                SIID_MEDIAENHANCEDCD = 87,
                SIID_MEDIAENHANCEDDVD = 88,
                SIID_MEDIAHDDVD = 89,
                SIID_MEDIABLURAY = 90,
                SIID_MEDIAVCD = 91,
                SIID_MEDIADVDPLUSR = 92,
                SIID_MEDIADVDPLUSRW = 93,
                SIID_DESKTOPPC = 94,
                SIID_MOBILEPC = 95,
                SIID_USERS = 96,
                SIID_MEDIASMARTMEDIA = 97,
                SIID_MEDIACOMPACTFLASH = 98,
                SIID_DEVICECELLPHONE = 99,
                SIID_DEVICECAMERA = 100,
                SIID_DEVICEVIDEOCAMERA = 101,
                SIID_DEVICEAUDIOPLAYER = 102,
                SIID_NETWORKCONNECT = 103,
                SIID_INTERNET = 104,
                SIID_ZIPFILE = 105,
                SIID_SETTINGS = 106,
                SIID_DRIVEHDDVD = 132,
                SIID_DRIVEBD = 133,
                SIID_MEDIAHDDVDROM = 134,
                SIID_MEDIAHDDVDR = 135,
                SIID_MEDIAHDDVDRAM = 136,
                SIID_MEDIABDROM = 137,
                SIID_MEDIABDR = 138,
                SIID_MEDIABDRE = 139,
                SIID_CLUSTEREDDRIVE = 140,
                SIID_MAX_ICONS = 175
            }

            [Flags]
            public enum SHGSI : uint
            {
                SHGSI_ICONLOCATION = 0,
                SHGSI_ICON = 0x000000100,
                SHGSI_SYSICONINDEX = 0x000004000,
                SHGSI_LINKOVERLAY = 0x000008000,
                SHGSI_SELECTED = 0x000010000,
                SHGSI_LARGEICON = 0x000000000,
                SHGSI_SMALLICON = 0x000000001,
                SHGSI_SHELLICONSIZE = 0x000000004
            }

            [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct SHSTOCKICONINFO
            {
                public UInt32 cbSize;
                public IntPtr hIcon;
                public Int32 iSysIconIndex;
                public Int32 iIcon;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x00000104)]
                public string szPath;
            }

            [DllImport("Shell32.dll", SetLastError = false, PreserveSig=false)]
            public static extern void SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

            public static System.Drawing.Icon GetLargeIcon(SHSTOCKICONID id)
            {
                SHSTOCKICONINFO shinfo = new SHSTOCKICONINFO();
                shinfo.cbSize = (UInt32)Marshal.SizeOf(shinfo);
                SHGetStockIconInfo(id, SHGSI.SHGSI_ICON | SHGSI.SHGSI_LARGEICON, ref shinfo);
                Icon ret = Icon.FromHandle(shinfo.hIcon);
                //DestroyIcon(shinfo.hIcon);
                return ret;
            }

            public static System.Drawing.Icon GetSmallIcon(SHSTOCKICONID id)
            {
                SHSTOCKICONINFO shinfo = new SHSTOCKICONINFO();
                shinfo.cbSize = (UInt32)Marshal.SizeOf(shinfo);
                SHGetStockIconInfo(id, SHGSI.SHGSI_ICON | SHGSI.SHGSI_SMALLICON, ref shinfo);
                Icon ret = Icon.FromHandle(shinfo.hIcon);
                //DestroyIcon(shinfo.hIcon);
                return ret;
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

                var roots = WorkingFolder.GetWorkspaceRoots(m_workspace.Folders);
                foreach(String root in roots)
                {
                    if (FileSpec.IsSubItem(initialPath, root))
                    {
                        string[] folders = initialPath.Substring(root.Length+1).Split('\\');
                        string curPath = root;
                        string prevPath = root;
                        foundItem = null;
                        m_navigateToWhenLoaded = null;
                        for (int i = 0; i < folders.Length; i++)
                        {
                            prevPath = curPath;
                            curPath = curPath + "\\" + folders[i];
                            if (TryFindNodeByServerItem(curPath, foundItem, out foundItem))
                            {
                                foundItem.EnsureVisible();
                            }
                            else
                            {
                                m_toExpand.Add(prevPath);
                                m_navigateToWhenLoaded = initialPath;
                            }
                        }
                        StartBackgroundWorkerIfNeeded();
                        break;
                    }
                }
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
            TreeNodeLocalItem root = AttachTreeNode(null, m_workspace != null ? m_workspace.Name : "wat?", new TempItemSet.TempItem());
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
                if (tempList.Count > 0)
                {
                    ExtendedItem[][] itemSets = m_workspace.GetExtendedItems(tempList.ToArray(), DeletedState.Any, ItemType.Any);
                    for (int j = 0; j < itemSets.Length; j++)
                    {
                        if (itemSets[j].Length > 0)
                        {
                            result[i].Items[tempIndx[j]].ServerItem = itemSets[j][0];
                        }
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
                if (string.IsNullOrEmpty(titem.LocalItem))
                {
                    e.Graphics.DrawIcon(TFV.Properties.Resources.TFSServer, new Rectangle(drawLoc, imageSize));
                }
                else
                {
                    bool isFolder = false;
                    if (item != null)
                        isFolder = item.ItemType == ItemType.Folder;
                    else
                        isFolder = Directory.Exists(titem.LocalItem);

                    if (isFolder)
                    {
                        e.Graphics.DrawIcon(titem.IsExpanded ? m_folderOpen : m_folderClosed, new Rectangle(drawLoc, imageSize));
                    }
                    else
                    {
                        e.Graphics.DrawIcon((item != null) ? m_addedFileIcon : m_fileIcon, new Rectangle(drawLoc, imageSize));
                    }
                }
                
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
                foreach (TreeNode n in e.Node.Nodes)
                {
                    if (n is TreeNodeLocalItem)
                    {
                        PurgeSelectionReferences((TreeNodeLocalItem)n);
                    }
                }
                e.Node.Nodes.Clear();
                e.Node.Nodes.Add(new TreeNodeWorking());
            }
        }

        void PurgeSelectionReferences(TreeNodeLocalItem root)
        {
            if (root.IsMultiSelect)
            {
                m_selectedItems[root.LocalItem] = null;
            }
            foreach(TreeNode n in root.Nodes)
            {
                if (n is TreeNodeLocalItem)
                {
                    PurgeSelectionReferences((TreeNodeLocalItem)n);
                }
            }
        }
    }
}
