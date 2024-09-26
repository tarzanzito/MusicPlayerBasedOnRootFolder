
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerPlayListResult
    {
        public string Files { get; private set; }

        public WorkerPlayListResult(string files)
        {
            Files = files;
        }
    }
}
