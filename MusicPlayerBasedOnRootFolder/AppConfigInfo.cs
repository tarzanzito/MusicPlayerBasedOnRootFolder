
namespace MusicManager
{
    internal class AppConfigInfo
    {
        public string FolderRoot { get; private set; } //loaded from main arg0
        public string MusicPlayerApplication { get; private set; }
        public string AudioFileExtensions { get; private set; } = ".MP3|.FLAC|";
           
        public AppConfigInfo(string folderRoot, string musicPlayerApplication, string audioFileExtensions)
        {
            FolderRoot = folderRoot;
            MusicPlayerApplication = musicPlayerApplication;

            if (!string.IsNullOrEmpty(audioFileExtensions))
                AudioFileExtensions = audioFileExtensions;
        }
    }
}
