using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using DG.Tweening;

public class CameraController : SceneService
{
    
    
    [SerializeField] private Transform _mainCamera;
    [SerializeField] private float _cameraMoveDuration;

    private GameStateService _gameStateService;
    private Vector3 _cameraUpPosition = new(0, 10, -10);
    private Vector3 _cameraDownPosition = new(0, 0, -10);

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();

        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
    }

    private void MainMenu_OnEnter(GameState state)
    {
        MoveCameraDownImmediate();
    }

    public void MoveCameraUpImmediate()
    {
        _mainCamera.localPosition = _cameraUpPosition;
    }
    
    public void MoveCameraDownImmediate()
    {
        _mainCamera.localPosition = _cameraDownPosition;
    }

    public void MoveCameraUpSmoothly()
    {
        _mainCamera.DOLocalMoveY(10, _cameraMoveDuration).SetEase(Ease.OutExpo);
    }

    public void MoveCameraDownSmoothly()
    {
        _mainCamera.DOLocalMoveY(0, _cameraMoveDuration).SetEase(Ease.OutExpo);
    }

    public void DOMoveYCamera(float targetPosition, float duration, Ease ease)
    {
        _mainCamera.DOLocalMoveY(targetPosition, duration).SetEase(ease);
    }
}
