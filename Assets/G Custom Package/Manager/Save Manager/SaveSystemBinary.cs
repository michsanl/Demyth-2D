using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CustomCode {
    public static class SaveSystemBinary
    {
        public static void Save<T> (T objectToSave, string Key)
        {
            string path = Application.persistentDataPath + "/Saves/";
            Directory.CreateDirectory(path);
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(path + Key + ".txt", FileMode.Create))
            {
                formatter.Serialize(fileStream, objectToSave);
            }
        }

        public static T Load<T>(string Key)
        {
            string path = Application.persistentDataPath + "/Saves/";
            BinaryFormatter formatter = new BinaryFormatter();
            T returnValue = default(T);

            using (FileStream fileStream = new FileStream(path + Key + ".txt", FileMode.Open))
            {
                returnValue = (T)formatter.Deserialize(fileStream);
            }

            return returnValue;
        }

        public static bool SaveExist(string key)
        {
            string path = Application.persistentDataPath + "/Saves/" + key + ".txt";
            return File.Exists(path);
        }
    }
}
