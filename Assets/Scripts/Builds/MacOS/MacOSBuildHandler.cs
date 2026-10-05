using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Protobot.Builds.MacOS
{
    public class MacOSBuildHandler : MonoBehaviour, IBuildHandler
    {
        public void Save(BuildData buildData)
        {
            BinaryFormatter bf = new BinaryFormatter();

            string fileLocation = GetFileLocation(buildData);
            var directory = Path.GetDirectoryName(fileLocation);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileStream file = File.Create(fileLocation);
            bf.Serialize(file, buildData);
            file.Close();
        }

        public void Delete(BuildData buildData)
        {
            var fileLocation = GetFileLocation(buildData);
            File.Delete(fileLocation);
        }

        public string GetFileLocation(BuildData buildData)
        {
            return Path.Combine(MacOSSavingConfig.saveDirectoryPath, buildData.fileName + MacOSSavingConfig.saveFileType);
        }

        public DateTime GetExactWriteTime(BuildData buildData)
        {
            var fileLocation = GetFileLocation(buildData);
            return File.GetLastWriteTime(fileLocation);
        }
    }
}
