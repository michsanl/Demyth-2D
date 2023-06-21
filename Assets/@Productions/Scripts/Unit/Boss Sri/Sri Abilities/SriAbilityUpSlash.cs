using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class SriAbilityUpSlash : MonoBehaviour
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
    [SerializeField] private GameObject upSlashCollider;
    
    private int UP_SLASH = Animator.StringToHash("Up_Slash");

    public IEnumerator UpSlash(Player player)
    {
        var playerYPosition = player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(UP_SLASH);
        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        upSlashCollider.SetActive(true);
        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        upSlashCollider.SetActive(false);
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
