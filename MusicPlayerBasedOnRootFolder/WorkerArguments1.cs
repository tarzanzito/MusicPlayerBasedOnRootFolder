
namespace MusicManager
{
    internal class WorkerArguments1
    {
        public string FolderRoot { get; private set; }

        public WorkerArguments1(string folderRoot)
        {
            FolderRoot = folderRoot.Trim();
        }
    }
}
