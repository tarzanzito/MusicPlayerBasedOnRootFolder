
namespace MusicManager
{
    internal class WorkerArguments
    {
        public string FolderRoot { get; private set; }

        public WorkerArguments(string folderRoot)
        {
            FolderRoot = folderRoot.Trim();
        }
    }
}
