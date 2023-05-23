
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerResult1
    {
        public TreeNode TreeNodeRoot{ get; private set; }
        public string FolderRoot { get; private set; }

        public WorkerResult1(TreeNode treeNodeRoot, string folderRoot)
        {
            TreeNodeRoot = treeNodeRoot;
            FolderRoot = folderRoot;
        }
    }
}
