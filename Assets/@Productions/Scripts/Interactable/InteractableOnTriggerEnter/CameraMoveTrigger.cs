using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;

public class CameraMoveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cameraGO;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveDuration;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        cameraGO.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutExpo);
    }
}
