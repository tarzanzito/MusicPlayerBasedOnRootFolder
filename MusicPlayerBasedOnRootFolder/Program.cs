using MusicManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MusicManager
{
    internal static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string arg0 = null;
            if (args.Length == 1)
                arg0 = args[0];

            AppConfigInfo appConfigInfo = ReadFromConfigFile(arg0);
            if (appConfigInfo == null)
                return 1;

            Application.Run(new FormMain(appConfigInfo));

            return 0;   
        }

        private static AppConfigInfo ReadFromConfigFile(string arg0)
        {
            //MusicPlayerApplication
            string musicPlayerApplication = System.Configuration.ConfigurationManager.AppSettings["MusicPlayerApplication"];
            if (musicPlayerApplication != null)
            {
                musicPlayerApplication = musicPlayerApplication.Trim();
                if (musicPlayerApplication == "")
                    musicPlayerApplication = null;
                else
                {
                    if (!File.Exists(musicPlayerApplication))
                    {
                        MessageBox.Show($"'App.Config' entry 'MusicPlayerApplication' points to file that does not exist. [{musicPlayerApplication}]", "App.Config ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                }
            }

            //AudioFileExtensions
            string audioFileExtensions = System.Configuration.ConfigurationManager.AppSettings["AudioFileExtensions"];
            if (audioFileExtensions != null)
            {
                audioFileExtensions = audioFileExtensions.Trim();
                if (audioFileExtensions == "")
                    audioFileExtensions = null;
            }

            if (audioFileExtensions == null)
            {
                    MessageBox.Show($"'App.Config' entry 'AudioFileExtensions' is empty (minimal = '.MP3:.FLAC:')", "App.Config ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
            }

            return new AppConfigInfo(arg0, musicPlayerApplication, audioFileExtensions);
        }
    }
}










//[STAThread]
//public static int Main()
//{
//    Application.EnableVisualStyles();
//    Application.SetCompatibleTextRenderingDefault(false);

//    AppConfigInfo appConfigInfo = ReadFromConfigFile();
//    if (appConfigInfo == null)
//        return 1;

//    Application.Run(new Form1(appConfigInfo));

//    return 0;
//}


//    }
//}
