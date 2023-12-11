using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrushers
{

    /// This is a starter template for Save System savers. To use it,
    /// make a copy, rename it, and remove the line marked above.
    /// Then fill in your code where indicated below.
    public class ScaleSaver : Saver // Rename this class.
    {

        /// A common approach is to store data to save in a class or struct, such as the one below.
        /// Mark it [Serializable] so the Save System can serialize it.

        [Tooltip("If set, save position of target. Otherwise save this GameObject's position.")]
        [SerializeField]
        private Transform m_target = null;

        [Serializable]
        public class ScaleData
        {
            public int scene = -1;
            public Vector3 scale;
        }

        protected ScaleData m_data = new();

        public Transform target
        {
            get { return (m_target == null) ? this.transform : m_target; }
            set { m_target = value; }
        }

        public override string RecordData()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            m_data.scene = currentScene;
            m_data.scale = target.transform.localScale;
            return SaveSystem.Serialize(m_data);
        }

        public override void ApplyData(string s)
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var data = SaveSystem.Deserialize<ScaleData>(s, m_data);
            if (data == null) return;
            m_data = data;
            SetPosition(data.scale);
            if (data.scene == currentScene || data.scene == -1)
            {
                SetPosition(data.scale);
            }
        }

        private void SetPosition(Vector3 scale)
        {
            target.transform.localScale = scale;
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

/**/
