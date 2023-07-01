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
    [SerializeField] private int topArenaBorder;
    [SerializeField] private int bottomArenaBorder;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upChargeCollider;
    
    private int UP_CHARGE = Animator.StringToHash("Up_charge");

    public IEnumerator UpCharge()
    {
        var playerYPosition = Context.Player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(UP_CHARGE);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        upChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        upChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private float GetPositionWithIncrement(float playerYPosition)
    {
        return playerYPosition + 2;
    }

    private float ClampValueToBattleArenaBorder(float value)
    {
        return Mathf.Clamp(value, bottomArenaBorder, topArenaBorder);
    }
}
