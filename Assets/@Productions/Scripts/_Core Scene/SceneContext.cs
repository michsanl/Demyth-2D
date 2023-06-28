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
        public HUDUI HUDUI;

        [Title("Gameplay")]
        public Player Player;

        [Title("Control System")]
        public GameManager gameManager;
        public GameInput gameInput;
        public LevelManager LevelManager;

        [Title("Audio")]
        public AudioManager AudioManager;

        [Title("Audio")]
        public GameObject VCamCameraShake;
    }
}