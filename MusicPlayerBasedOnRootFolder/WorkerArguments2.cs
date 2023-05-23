
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerArguments2
    {
        public TreeNode TreeNodeRoot { get; private set; }
        public string FullPathRoot { get; private set; }

        public WorkerArguments2(TreeNode treeNodeRoot, string fullPathRoot)
        {
            TreeNodeRoot = treeNodeRoot;
            FullPathRoot = fullPathRoot;
        }
    }
}
