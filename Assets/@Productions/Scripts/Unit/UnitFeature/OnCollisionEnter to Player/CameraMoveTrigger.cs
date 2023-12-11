using UnityEngine;
using DG.Tweening;

public class CameraMoveTrigger : MonoBehaviour
{
    [SerializeField] private int targetPositionY;
    [SerializeField] private float moveDuration;

    [SerializeField]
    private Transform cameraNormal;

    private void OnCollisionEnter(Collision other) 
    {
        cameraNormal.DOLocalMoveY(targetPositionY, moveDuration).SetEase(Ease.OutExpo);
    }
}
