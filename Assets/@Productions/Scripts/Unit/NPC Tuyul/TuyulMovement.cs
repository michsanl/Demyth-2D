using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class TuyulMovement : MonoBehaviour
{
    
    [SerializeField] private float actionDelay = .25f;
    [SerializeField] private float moveDuration = .245f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask yulaPathLayerMask;
    [SerializeField] private LayerMask moveBlockerLayerMask;

    private LookOrientation lookOrientation;
    private bool isBusy;
    private Vector3[] scanDirUpRightLeft = new Vector3[3] { Vector3.up, Vector3.right, Vector3.left};
    private Vector3[] scanDirDownRightLeft = new Vector3[3] { Vector3.down, Vector3.right, Vector3.left};
    private Vector3[] scanDirUpDownRight = new Vector3[3] { Vector3.up, Vector3.down, Vector3.right};
    private Vector3[] scanDirUpDownLeft = new Vector3[3] { Vector3.up, Vector3.down, Vector3.left};

    private void Awake() 
    {
        lookOrientation = GetComponent<LookOrientation>();
    }

    public IEnumerator Flee(Vector3 directionToPlayer)
    {
        if (isBusy)
            yield break;

        HandleFlee(directionToPlayer);
    }

    private void HandleFlee(Vector3 directionToPlayer)
    {
        if (directionToPlayer == Vector3.up)
        {
            FleeFromUp();
            return;
        }

        if (directionToPlayer == Vector3.down)
        {
            FleeFromDown();
            return;
        }

        if (directionToPlayer == Vector3.right)
        {
            FleeFromRight();
            return;
        }

        if (directionToPlayer == Vector3.left)
        {
            FleeFromLeft();
            return;
        }
    }

    private void FleeFromRight()
    {
        foreach (Vector3 dir in scanDirUpDownLeft)
        {
            TryFlee(dir);
        }
    }

    private void FleeFromLeft()
    {
        foreach (Vector3 dir in scanDirUpDownRight)
        {
            TryFlee(dir);
        }
    }

    private void FleeFromDown()
    {
        foreach (Vector3 dir in scanDirUpRightLeft)
        {
            TryFlee(dir);
        }
    }

    private void FleeFromUp()
    {
        foreach (Vector3 dir in scanDirDownRightLeft)
        {
            TryFlee(dir);
        }
    }

    private void TryFlee(Vector3 dir)
    {
        var hit = Physics2D.Raycast(transform.position + dir, dir, .1f, yulaPathLayerMask);
        if (hit.collider != null)
        {
            lookOrientation.SetFacingDirection(dir);
            StartCoroutine(Move(dir));
        }
    }

    private IEnumerator Move(Vector3 moveDir)
    {
        if (IsMovePathBlocked(moveDir))
            yield break;

        isBusy = true;

        transform.DOMove(GetMoveTargetPosition(moveDir), moveDuration);
        animator.SetTrigger("Dash");
        yield return Helper.GetWaitForSeconds(actionDelay);

        isBusy = false;
    }

    private RaycastHit2D IsMovePathBlocked(Vector3 moveDir)
    {
        return Physics2D.Raycast(transform.position + moveDir, moveDir, .1f, moveBlockerLayerMask);
    }

    private Vector3 GetMoveTargetPosition(Vector3 moveDir)
    {
        var moveTargetPosition = transform.position + moveDir;
        moveTargetPosition.x = Mathf.RoundToInt(moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt(moveTargetPosition.y);
        return moveTargetPosition;
    }

}
