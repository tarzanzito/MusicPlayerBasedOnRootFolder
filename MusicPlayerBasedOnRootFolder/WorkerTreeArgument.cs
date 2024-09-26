
namespace MusicManager
{
    internal class WorkerTreeArgument
    {
        public string FolderRoot { get; private set; }

        public WorkerTreeArgument(string folderRoot)
        {
            FolderRoot = folderRoot.Trim();
        }
    }
}
