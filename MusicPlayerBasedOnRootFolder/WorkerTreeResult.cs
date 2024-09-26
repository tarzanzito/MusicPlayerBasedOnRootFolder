
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerTreeResult
    {
        public TreeNode TreeNodeRoot{ get; private set; }
        public string RootPath { get; private set; }
        public string RootName { get; private set; }

        public WorkerTreeResult(TreeNode treeNodeRoot, string rootName, string rootPath)
        {
            TreeNodeRoot = treeNodeRoot;
            RootName = rootName;
            RootPath = rootPath;
        }
    }
}
