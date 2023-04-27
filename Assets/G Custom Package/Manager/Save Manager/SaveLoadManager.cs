using CustomCode.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomCode
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        public string defaultSaveFileName = "MRS Game";
        private string lastFileName = "";
        public bool autoSave = false;
        [Range(1, 20)]
        public float autoSaveInterval = 5f; //In Minute
        private List<ISaveData> saveDatas = new List<ISaveData>();

        protected override void Awake()
        {
            base.Awake();
            SaveJSON.Init();
        }

        private void Start()
        {
            if (autoSave)
            {
                InvokeRepeating(nameof(AutoSave), autoSaveInterval.MinuteToSecond(), autoSaveInterval.MinuteToSecond());
            }
        }

        public void InitializeSaveData(ISaveData saveData)
        {
            saveDatas.Add(saveData);
        }

        private void OnApplicationQuit()
        {
            Save(lastFileName);
        }

        private void AutoSave()
        {
            Save(lastFileName);
            Debug.Log("Auto save");
        }

        public void Save(string saveFileName = "")
        {
            if (string.IsNullOrEmpty(saveFileName))
            {
                saveFileName = defaultSaveFileName;
            }

            lastFileName = saveFileName;

            var wrappers = saveDatas.Select(saveData => new JsonWrapper
            {
                uniqueName = saveData.GetUniqueName(), dataValue = JsonUtility.ToJson(saveData.GetSaveData())
            }).ToList();

            SaveJSON.SaveJSONData(saveFileName, ToJsonWrapper(wrappers));

            Debug.Log($"Save data with name : {saveFileName} with {wrappers}");
        }

        public void Load(string loadFileName = "")
        {
            if (string.IsNullOrEmpty(loadFileName))
                loadFileName = defaultSaveFileName;

            if (!SaveJSON.IsFileExist(loadFileName))
            {
                return;
            }

            lastFileName = loadFileName;
            var savedJson = SaveJSON.LoadJSONData(loadFileName);
            var wrappers = FromJsonWrapper<JsonWrapper>(savedJson);

            foreach (var saveData in saveDatas)
            {
                var wrapper = wrappers.Where(wrapper => wrapper.uniqueName == saveData.GetUniqueName()).FirstOrDefault();
                if (wrapper != null)
                {
                    var data = JsonUtility.FromJson(wrapper.dataValue, saveData.GetSaveDataType());
                    if(data != null)
                    {
                        saveData.ResetData();
                        saveData.OnLoad(data);
                    }
                }
            }
        }

        private List<T> FromJsonWrapper<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        private string ToJsonWrapper<T>(List<T> list, bool prettyPrint = false)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = list;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        protected override bool ShouldNotDestroyOnLoad()
        {
            return false;
        }

        [System.Serializable]
        public class JsonWrapper
        {
            public string uniqueName;
            public string dataValue;
        }

        [System.Serializable]
        public class Wrapper<T>
        {
            public List<T> items;
        }
    }
}
