using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Core;

namespace Demyth.Gameplay
{
    public class Gate : Interactable
    {
        private enum CameraMoveDirection { Up, Down };
        
        public Vector3 EnterPoint => transform.position;

        [SerializeField]
        private EnumId targetLevel;
        [SerializeField] 
        private bool moveCameraOnLevelChange;
        [SerializeField, ShowIf("moveCameraOnLevelChange")]
        private CameraMoveDirection cameraMoveDirection;

        private Level _level;
        private CameraController _cameraController;

        private void Awake()
        {
            _cameraController = SceneServiceProvider.GetService<CameraController>();
        }

        public void SetupGate(Level level)
        {
            _level = level;
        }

        public override void Interact(Player player, Vector3 direction = default)
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
            {
                _cameraController.MoveCameraUpImmediate();
            }
            else
            {
                _cameraController.MoveCameraDownImmediate();
            }
        }
    }
}
