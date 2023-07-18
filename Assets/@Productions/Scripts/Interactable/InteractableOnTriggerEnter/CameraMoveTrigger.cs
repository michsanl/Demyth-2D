using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class CameraMoveTrigger : SceneService
{
    [SerializeField] private int targetPositionY;
    [SerializeField] private float moveDuration;

    protected override void OnActivate()
    {
        Context.GameManager.OnOpenMainMenu += GameManager_OnOpenMainMenu;
    }

    private void GameManager_OnOpenMainMenu()
    {
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        Context.CameraNormal.transform.DOLocalMoveY(targetPositionY, moveDuration).SetEase(Ease.OutExpo);
    }
}
