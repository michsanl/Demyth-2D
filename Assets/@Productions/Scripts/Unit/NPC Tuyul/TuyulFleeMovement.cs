using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class TuyulFleeMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask yulaPathLayerMask;
    [SerializeField] private LayerMask moveBlockerLayerMask;

    private LookOrientation lookOrientation;
    private bool isBusy;

    private void Awake() 
    {
        lookOrientation = GetComponent<LookOrientation>();
    }

    public void Flee(Vector3 directionToPlayer)
    {
        if (isBusy)
            return;

        TryFlee(directionToPlayer);
    }

    public void TryFlee(Vector3 dirToPlayer)
    {
        var fleeDir = dirToPlayer;
        int loopCount = 3;
        for (int i = 0; i < loopCount; i++)
        {
            fleeDir = Vector2.Perpendicular(fleeDir);
            if (IsFleePathAvailable(fleeDir) && !IsPathBlocked(fleeDir))
            {
                // can flee
                lookOrientation.SetFacingDirection(fleeDir);
                StartCoroutine(Move(fleeDir));
                return;
            }
        }
        // cant flee
        // set panic animation
        lookOrientation.SetFacingDirection(dirToPlayer);
    }

    private bool IsFleePathAvailable(Vector3 dirToCheck)
    {
        return Physics2D.Raycast(transform.position + dirToCheck, dirToCheck, .1f, yulaPathLayerMask);
    }

    private bool IsPathBlocked(Vector3 dirToCheck)
    {
        return Physics2D.Raycast(transform.position + dirToCheck, dirToCheck, .1f, moveBlockerLayerMask);
    }

    private IEnumerator Move(Vector3 moveDir)
    {
        isBusy = true;

        animator.SetTrigger("Dash");
        transform.DOMove(GetMoveTargetPositionRounded(moveDir), moveDuration);
        yield return Helper.GetWaitForSeconds(moveDuration);

        isBusy = false;
    }

    private Vector3 GetMoveTargetPositionRounded(Vector3 moveDir)
    {
        var moveTargetPosition = transform.position + moveDir;
        moveTargetPosition.x = Mathf.RoundToInt(moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt(moveTargetPosition.y);
        return moveTargetPosition;
    }

}
