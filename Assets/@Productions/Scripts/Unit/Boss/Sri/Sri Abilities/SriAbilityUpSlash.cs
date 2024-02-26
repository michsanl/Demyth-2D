using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;
using MoreMountains.Tools;
using Core;
using Demyth.Gameplay;

public class SriAbilityUpSlash : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _upSlashProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upSlashCollider;
    
    private Player _player;
    private int topArenaBorder = 2;
    private int bottomArenaBorder = -4;
    private int UP_SLASH = Animator.StringToHash("Up_Slash");

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public override IEnumerator PlayAbility()
    {
        animator.SetFloat("Ver_Slash_Multiplier", _upSlashProp.AnimationSpeedMultiplier);
        
        var playerYPosition = _player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.SetTrigger(UP_SLASH);
        Helper.PlaySFX(_sriClipSO.VerticalSlash, _sriClipSO.VerticalSlashVolume);

        yield return Helper.GetWaitForSeconds(_upSlashProp.GetFrontSwingDuration());
        upSlashCollider.SetActive(true);
        yield return transform.DOMoveY(finalTargetPosition, _upSlashProp.GetSwingDuration()).SetEase(animationCurve).WaitForCompletion();
        upSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_upSlashProp.GetBackSwingDuration());
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
