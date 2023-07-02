using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityHorizontalCharge : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject horizontalChargeCollider;
    
    private int HORIZONTAL_CHARGE = Animator.StringToHash("Side_charge");

    public IEnumerator HorizontalCharge()
    {
        var targetPosition = Context.Player.transform.position.x;
        if (targetPosition > transform.position.x)
        {
            targetPosition = SetPositionToPlayerRight(targetPosition);
            targetPosition = RightClampToMoveBlocker(targetPosition);
        }
        else
        {
            targetPosition = SetPositionToPlayerLeft(targetPosition);
            targetPosition = LeftClampToMoveBlocker(targetPosition);
        }
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(HORIZONTAL_CHARGE);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        horizontalChargeCollider.SetActive(true);

        yield return transform.DOMoveX(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        horizontalChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private float SetPositionToPlayerRight(float targetPosition)
    {
        return targetPosition + 2;
    }

    private float SetPositionToPlayerLeft(float targetPosition)
    {
        return targetPosition - 2;
    }

    private float RightClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, transform.position.x, GetClampMaxValue());
    }

    private float LeftClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, GetClampMinValue(), transform.position.x);
    }

    private float GetClampMaxValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right, Vector2.right, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.x - .5f;
    }

    private float GetClampMinValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.left, Vector2.left, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.x + .5f;
    }
}
