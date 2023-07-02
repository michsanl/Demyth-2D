using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;

public class PetraAbilityUpCharge : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upChargeCollider;
    
    private int UP_CHARGE = Animator.StringToHash("Up_charge");

    public IEnumerator UpCharge()
    {
        var targetPosition = Context.Player.transform.position.y;
        targetPosition = SetPositionToBehindPlayer(targetPosition);
        targetPosition = ClampToMoveBlocker(targetPosition);
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(UP_CHARGE);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        upChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        upChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private float SetPositionToBehindPlayer(float playerYPosition)
    {
        return playerYPosition + 2;
    }

    private float ClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, transform.position.y, GetClampMaxValue());
    }

    private float GetClampMaxValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.up, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.y - .5f;
    }
}
