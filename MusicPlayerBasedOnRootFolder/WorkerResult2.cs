
using System.Windows.Forms;

namespace MusicManager
{
    internal class WorkerResult2
    {
        public string Files { get; private set; }

        public WorkerResult2(string files)
        {
            Files = files;
        }
    }
}
