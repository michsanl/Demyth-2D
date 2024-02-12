using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using Core;
using Demyth.Gameplay;

public class SriAbilityHorizontalSlash : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _horSlashProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject horizontalSlashCollider;
    
    private Player _player;
    private int rightArenaBorder = 6;
    private int leftArenaBorder = -6;
    protected int HORIZONTAL_SLASH = Animator.StringToHash("Horizontal_Slash");

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Hor_Slash_Multiplier", _horSlashProp.AnimationSpeedMultiplier);
        
        float playerXPosition = _player.transform.position.x;
        float targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerXPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        _animator.SetTrigger(HORIZONTAL_SLASH);
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
