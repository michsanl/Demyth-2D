using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilityHorizontalSlash : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _horSlashProp;
    [SerializeField] private GameObject horizontalSlashCollider;
    [SerializeField] private SriClipSO _sriClipSO;
    
    private int rightArenaBorder = 6;
    private int leftArenaBorder = -6;
    protected int HORIZONTAL_SLASH = Animator.StringToHash("Horizontal_Slash");

    public IEnumerator HorizontalSlash(Player player, Animator animator)
    {
        animator.SetFloat("Hor_Slash_Multiplier", _horSlashProp.AnimationSpeedMultiplier);
        
        float playerXPosition = player.transform.position.x;
        float targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerXPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.SetTrigger(HORIZONTAL_SLASH);
        Helper.PlaySFX(_sriClipSO.HorizontalSlash, _sriClipSO.HorizontalSlashVolume);

        yield return Helper.GetWaitForSeconds(_horSlashProp.GetFrontSwingDuration());
        horizontalSlashCollider.SetActive(true);
        yield return transform.DOMoveX(finalTargetPosition, _horSlashProp.GetSwingDuration()).SetEase(animationCurve).WaitForCompletion();
        horizontalSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_horSlashProp.GetBackSwingDuration());
    }

    private float GetPositionWithIncrement(float playerXPosition)
    {
        return playerXPosition > transform.position.x ? playerXPosition + 2 : playerXPosition -2;
    }

    private float ClampValueToBattleArenaBorder(float value)
    {
        return Mathf.Clamp(value, leftArenaBorder, rightArenaBorder);
    }
}
