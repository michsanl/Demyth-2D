using CustomTools.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

namespace Demyth.Gameplay
{
    public class Gate : Interactable
    {
        public Vector3 EnterPoint => transform.position;
        [SerializeField, ValueDropdown(nameof(levelNameList))]
        private string targetLevel;
        [SerializeField] private bool moveCameraOnLevelChange;
        [SerializeField, ShowIf("moveCameraOnLevelChange")]
        private CameraMoveDirection cameraMoveDirection;
        [SerializeField, ShowIf("moveCameraOnLevelChange")]
        private GameObject cameraGO;

        private enum CameraMoveDirection { Up, Down };

        private List<string> levelNameList = new List<string> 
        { 
            "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level Main Menu" 
        }; 
        private Level _level;

        public void SetupGate(Level level)
        {
            _level = level;
        }

        public override void Interact(Vector3 direction = default)
        {
            _level.MoveToNextLevel(targetLevel);
            MoveCamera();

            PixelCrushers.SaveSystem.SaveToSlot(1);
        }

        private void MoveCamera()
        {
            if (!moveCameraOnLevelChange)
                return;

            if (cameraMoveDirection == CameraMoveDirection.Up)
                cameraGO.transform.localPosition = new Vector3(0, 10, 0); 
                
            if (cameraMoveDirection == CameraMoveDirection.Down)
                cameraGO.transform.localPosition = Vector3.zero;
        }

#if UNITY_EDITOR
        private IEnumerable<string> LevelName()
        {
            string levelPath = "Assets/@Productions/Prefabs/Level Map";
            string[] guids = UnityEditor.AssetDatabase.FindAssets("", new[] { levelPath });
            return guids
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(y => UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(y).name);
        }
#endif
    }
}