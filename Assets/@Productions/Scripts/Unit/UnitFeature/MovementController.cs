using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    public void Move(Vector3 playerDir, float moveDuration) 
    {
        Vector3 moveTarget = transform.position + playerDir;
        moveTarget.x = Mathf.Round(moveTarget.x);
        moveTarget.y = Mathf.Round(moveTarget.y);
        Helper.MoveToPosition(transform, moveTarget, moveDuration);
    }
}