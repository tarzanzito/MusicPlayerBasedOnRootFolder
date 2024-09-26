
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerPlayListArgument
    {
        public TreeNode TreeNodeRoot { get; private set; }
        public string FullPathRoot { get; private set; }
        public bool PlayAll { get; private set; }

        public WorkerPlayListArgument(TreeNode treeNodeRoot, string fullPathRoot, bool playAll)
        {
            TreeNodeRoot = treeNodeRoot;
            FullPathRoot = fullPathRoot;
            PlayAll = playAll;
        }
    }
}
