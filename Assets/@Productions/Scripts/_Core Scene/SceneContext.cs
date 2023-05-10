using Sirenix.OdinInspector;
using UISystem;
using UnityEngine;

namespace CustomTools.Core
{
    [System.Serializable]
    public class SceneContext
    {
        [Title("Scene UI")]
        public SceneUI UI;

        [Title("Gameplay")]
        public LevelManager LevelManager;
        public Player Player;
    }
}