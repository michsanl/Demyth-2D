using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TuyulPatrolMovement : MonoBehaviour
{
    [SerializeField] private float moveIntervalDelay; 
    [SerializeField] private float moveDuration;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;
    [SerializeField] private LayerMask playerMask;

    private void OnEnable() 
    {
        StartCoroutine(PatrolMovementLoop());
    }

    private IEnumerator PatrolMovementLoop()
    {
        Vector2 moveDir = GetMoveDir();

        if (IsMovePathBlocked(moveDir))
        {
            yield return Helper.GetWaitForSeconds(moveIntervalDelay);
        }
        else
        {
            SetFacingDirection(moveDir);
            yield return StartCoroutine(Move(moveDir));
        }

        yield return Helper.GetWaitForSeconds(moveIntervalDelay);

        StartCoroutine(PatrolMovementLoop());
    }

    private void SetFacingDirection(Vector2 moveDir)
    {
        if (moveDir.x != 0)
            model.transform.localScale = new Vector3(moveDir.x, model.localScale.y);
    }

    private bool IsMovePathBlocked(Vector2 moveDir)
    {
        return Physics.Raycast(transform.position, moveDir, 1.5f, playerMask);
    }

    private Vector2 GetMoveDir()
    {
        if (transform.position.y == 1 && transform.position.x < 4)
        {
            return Vector2.right;
        }
        if (transform.position.x == 4 && transform.position.y > -3)
        {
            return Vector2.down;
        }
        if (transform.position.y == -3 && transform.position.x > -4)
        {
            return Vector2.left;
        }
        if (transform.position.x == -4 && transform.position.y < 1)
        {
            return Vector2.up;
        }
        return Vector2.zero;
    }

    private IEnumerator Move(Vector3 moveDir)
    {
        animator.SetTrigger("Dash");
        Vector2 moveTargetPosition = GetRoundedMoveTargetPosition(moveDir);
        yield return transform.DOMove(moveTargetPosition, moveDuration).WaitForCompletion();
    }

    private Vector3 GetRoundedMoveTargetPosition(Vector3 moveDir)
    {
        var moveTargetPosition = transform.position + moveDir;
        moveTargetPosition.x = Mathf.RoundToInt((float)moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt((float)moveTargetPosition.y);
        return moveTargetPosition;
    }
}
