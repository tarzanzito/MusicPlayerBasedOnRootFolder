
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerArguments2
    {
        public TreeNode TreeNodeRoot { get; private set; }
        public string FullPathRoot { get; private set; }
        public bool PlayAll { get; private set; }

        public WorkerArguments2(TreeNode treeNodeRoot, string fullPathRoot, bool playAll)
        {
            TreeNodeRoot = treeNodeRoot;
            FullPathRoot = fullPathRoot;
            PlayAll = playAll;
        }
    }
}
