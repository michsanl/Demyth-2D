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

    private void OnCollisionEnter(Collision other) 
    {
        Context.CameraNormal.transform.DOLocalMoveY(targetPositionY, moveDuration).SetEase(Ease.OutExpo);
    }
}
