using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilityUpSlash : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private AbilityTimelineSO _abilityTimeline;
    [SerializeField] private GameObject upSlashCollider;
    [SerializeField] private Animator _animator;
    
    private int topArenaBorder = 2;
    private int bottomArenaBorder = -4;
    private int UP_SLASH = Animator.StringToHash("Up_Slash");

    private void Awake()
    {
        _animator.SetFloat("Ver_Slash_A", _abilityTimeline.AnticipationMultiplier);
        _animator.SetFloat("Ver_Slash_B", _abilityTimeline.AttackMultiplier);
        _animator.SetFloat("Ver_Slash_C", _abilityTimeline.RecoveryMultiplier);
    }

    public IEnumerator UpSlash(Player player, Animator animator, AudioClip abilitySFX)
    {
        var playerYPosition = player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.SetTrigger("Up_Slash");
        PlayAudio(abilitySFX);

        yield return Helper.GetWaitForSeconds(_abilityTimeline.FinalAnticiptionDuration);
        upSlashCollider.SetActive(true);
        yield return transform.DOMoveY(finalTargetPosition, _abilityTimeline.FinalAttackDuration).SetEase(animationCurve).WaitForCompletion();
        upSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_abilityTimeline.FinalRecoveryDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
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
