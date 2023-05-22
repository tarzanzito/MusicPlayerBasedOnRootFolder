
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerArguments2
    {
        public TreeNode TreeNodeRoot { get; private set; }
        public int NodeRootTextLenght { get; private set; }

        public WorkerArguments2(TreeNode treeNodeRoot, int nodeRootTextLenght)
        {
            TreeNodeRoot = treeNodeRoot;
            NodeRootTextLenght = nodeRootTextLenght;
        }
    }
}
