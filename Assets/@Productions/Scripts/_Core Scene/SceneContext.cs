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
        public GameManager GameManager;
        public GameInput GameInput;
        public LevelManager LevelManager;

        [Title("Audio")]
        public AudioManager AudioManager;

        [Title("Camera")]
        public GameObject CameraNormal;
        public CameraShakeController CameraShakeController;
    }
}