using UnityEngine;
using DG.Tweening;
using Core;
using System;

public class CameraMoveTrigger : MonoBehaviour
{
    public enum CameraMoveDir { Up, Down };
    
    [SerializeField] private CameraMoveDir _cameraMoveDir;

    private CameraController _cameraController;

    private void Awake()
    {
        _cameraController = SceneServiceProvider.GetService<CameraController>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other is null) return;

        if (_cameraMoveDir == CameraMoveDir.Up)
        {
            _cameraController.MoveCameraUpSmoothly();
        }
        else
        {
            _cameraController.MoveCameraDownSmoothly();
        }
    }
}
