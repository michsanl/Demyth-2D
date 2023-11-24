using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Core;

namespace Demyth.Gameplay
{
    public class Gate : Interactable
    {
        public Vector3 EnterPoint => transform.position;
        [SerializeField]
        private EnumId targetLevel;
        [SerializeField] private bool moveCameraOnLevelChange;
        [SerializeField, ShowIf("moveCameraOnLevelChange")]
        private CameraMoveDirection cameraMoveDirection;
        [SerializeField, ShowIf("moveCameraOnLevelChange")]
        private GameObject cameraGO;

        public static Action OnAnyGateInteract;

        private enum CameraMoveDirection { Up, Down };
        private Level _level;

        public void SetupGate(Level level)
        {
            _level = level;
        }

        public override void Interact(Player player, Vector3 direction = default)
        {
            OnAnyGateInteract?.Invoke();
            _level.MoveToNextLevel(targetLevel);
            MoveCamera();
            PixelCrushers.SaveSystem.SaveToSlot(1);
        }

        private void MoveCamera()
        {
            if (!moveCameraOnLevelChange)
                return;

            if (cameraMoveDirection == CameraMoveDirection.Up)
                cameraGO.transform.localPosition = new Vector3(0, 10, -10); 
                
            if (cameraMoveDirection == CameraMoveDirection.Down)
                cameraGO.transform.localPosition = new Vector3(0, 0, -10);
        }
    }
}
