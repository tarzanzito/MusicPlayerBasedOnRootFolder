
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerResult1
    {
        public TreeNode TreeNodeRoot{ get; private set; }

        public WorkerResult1(TreeNode treeNodeRoot)
        {
            TreeNodeRoot = treeNodeRoot;
        }
    }
}
