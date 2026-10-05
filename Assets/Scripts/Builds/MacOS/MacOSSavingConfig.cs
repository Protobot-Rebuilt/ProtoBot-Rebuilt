using UnityEngine;
using System.IO;

namespace Protobot.Builds.MacOS
{
    public static class MacOSSavingConfig
    {
        private const string AppFolderName = "Protobot Rebuilt.app";
        private const string BuildsDirectory = "Builds";

        public static string appDirectoryPath => Path.Combine(Application.persistentDataPath, AppFolderName);
        public static string saveDirectoryPath => Path.Combine(appDirectoryPath, BuildsDirectory);
        public static string saveFileType => ".build";

        private static bool OnMacOS => Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (OnMacOS)
            {
                EnsureDirectoryExists();
            }
        }

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(appDirectoryPath))
            {
                Directory.CreateDirectory(appDirectoryPath);
            }

            if (!Directory.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }
        }
    }
}
