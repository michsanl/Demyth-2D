using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PixelCrushers
{

    /// This is a starter template for Save System savers. To use it,
    /// make a copy, rename it, and remove the line marked above.
    /// Then fill in your code where indicated below.
    public class PlayerDataSaver : Saver // Rename this class.
    {

        /// A common approach is to store data to save in a class or struct, such as the one below.
        /// Mark it [Serializable] so the Save System can serialize it.
        [Serializable]
        public class PlayerData
        {
            public Vector3 Position;
            public Vector3 FacingDirection;
            public bool IsActive;
            public bool UsePan;
            public bool IsLanternUnlocked;
            public bool IsHealthPotionUnlocked;
            public bool IsShieldUnlocked;
        }

        [SerializeField]
        private Player m_playerToWatch;
        [SerializeField]
        private Transform m_playerModelToWatch;

        private PlayerData m_playerData = new();

        public override string RecordData()
        {
            /// This method should return a string that represents the data you want to save.
            /// You can use SaveSystem.Serialize() to serialize a serializable object to a 
            /// string. This will use the serializer component on the Save System GameObject,
            /// which defaults to Unity's built-in JSON serialization. Remember that Unity
			/// cannot directly serialize lists or arrays, so you must put them inside a
			/// class.
            /// 
            /// If you use a class to hold the data, use SaveSystem.Serialize to return a 
            /// serialized version:
            /// 
            //Data data = new Data();
            //data.someData = false;
            //return SaveSystem.Serialize(data);

            m_playerData.IsActive = m_playerToWatch.gameObject.activeSelf;
            m_playerData.Position = m_playerToWatch.transform.position;
            m_playerData.UsePan = m_playerToWatch.UsePan;
            m_playerData.IsLanternUnlocked = m_playerToWatch.IsLanternUnlocked;
            m_playerData.IsHealthPotionUnlocked = m_playerToWatch.IsHealthPotionUnlocked;
            m_playerData.IsShieldUnlocked = m_playerToWatch.IsShieldUnlocked;
            m_playerData.FacingDirection = m_playerModelToWatch.localScale;

            return SaveSystem.Serialize(m_playerData);
        }

        public override void ApplyData(string s)
        {
            /// This method should process the string representation of saved data and apply
            /// it to the current state of the game. You can use SaveSystem.Deserialize()
            /// to deserialize the string to an object that specifies the state to apply to
            /// the game.
            /// 
            //if (string.IsNullOrEmpty(s)) return; // No data to apply.
            //Data data = SaveSystem.Deserialize<Data>(s);
            //if (data == null) return; // Serialized string isn't valid.
            ////(do something with data here)

            if (m_playerToWatch == null || string.IsNullOrEmpty(s)) 
                return;
            var data = SaveSystem.Deserialize<PlayerData>(s);
            if (data == null) 
                return;
            m_playerData = data;

            m_playerToWatch.gameObject.SetActive(data.IsActive);
            m_playerToWatch.transform.position = data.Position;
            m_playerToWatch.UsePan = data.UsePan;
            m_playerToWatch.IsLanternUnlocked = data.IsLanternUnlocked;
            m_playerToWatch.IsHealthPotionUnlocked = data.IsHealthPotionUnlocked;
            m_playerToWatch.IsShieldUnlocked = data.IsShieldUnlocked;
            m_playerModelToWatch.localScale = data.FacingDirection;
        }

        //public override void ApplyDataImmediate()
        //{
        //    // If your Saver needs to pull data from the Save System immediately after
        //    // loading a scene, instead of waiting for ApplyData to be called at its
        //    // normal time, which may be some number of frames after the scene has started,
        //    // it can implement this method. For efficiency, the Save System will not look up 
        //    // the Saver's data; your method must look it up manually by calling 
        //    // SaveSystem.savedGameData.GetData(key).
        //}

        //public override void OnBeforeSceneChange()
        //{
        //    // The Save System will call this method before scene changes. If your saver listens for 
        //    // OnDisable or OnDestroy messages (see DestructibleSaver for example), it can use this 
        //    // method to ignore the next OnDisable or OnDestroy message since they will be called
        //    // because the entire scene is being unloaded.
        //}

        //public override void OnRestartGame()
        //{
        //    // The Save System will call this method when restarting the game from the beginning.
        //    // Your Saver can reset things to a fresh state if necessary.
        //}

    }

}
