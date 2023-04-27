using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomCode
{
    public static class SaveJSON
    {
        public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/SavesData/";

        public static void Init()
        {
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }

        public static void SaveJSONData(string fileName, string jsonString)
        {
            File.WriteAllText(SAVE_FOLDER + fileName + ".save", jsonString);
        }

        public static string LoadJSONData(string fileName)
        {
            if (IsFileExist(fileName))
            {
                string jsonData = File.ReadAllText(SAVE_FOLDER + fileName + ".save");
                return jsonData;
            }
            else
            {
                return null;
            }
        }

        public static bool IsFileExist(string fileName)
        {
            return File.Exists(SAVE_FOLDER + fileName + ".save");
        }
    }
}
