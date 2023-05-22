using System;

namespace MusicManager
{
    internal class WorkerProcessState
    {
        public string Folder { get; private set; }

        public WorkerProcessState(string folder)
        {
            Folder = folder;
        }
    }
}
