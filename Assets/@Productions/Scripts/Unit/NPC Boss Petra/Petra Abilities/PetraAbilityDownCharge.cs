using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using System;

public class PetraAbilityDownCharge : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject downChargeCollider;
    

    public IEnumerator DownCharge(Action OnStart)
    {
        OnStart?.Invoke();
        int moveTargetPosition = GetMoveTargetPosition();

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        downChargeCollider.SetActive(true);

        yield return transform.DOMoveY(moveTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        downChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private int GetMoveTargetPosition()
    {
        var targetPosition = Context.Player.transform.position.y;
        targetPosition = SetPositionToBehindPlayer(targetPosition);
        targetPosition = ClampToMoveBlocker(targetPosition);
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);
        return finalTargetPosition;
    }

    private float SetPositionToBehindPlayer(float playerYPosition)
    {
        return playerYPosition - 2f;
    }

    private float ClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, GetClampMinValue(), transform.position.y);
    }

    private float GetClampMinValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down, Vector2.down, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.y + .5f;
    }

}
