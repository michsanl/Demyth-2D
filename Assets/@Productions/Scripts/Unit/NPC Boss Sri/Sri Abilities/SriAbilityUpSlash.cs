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
    [SerializeField] private AnimationPropertiesSO _upSlashProp;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upSlashCollider;
    [SerializeField] private SriClipSO _sriClipSO;
    
    private int topArenaBorder = 2;
    private int bottomArenaBorder = -4;
    private int UP_SLASH = Animator.StringToHash("Up_Slash");

    public IEnumerator UpSlash(Player player, Animator animator)
    {
        animator.SetFloat("Ver_Slash_Multiplier", _upSlashProp.AnimationSpeedMultiplier);
        
        var playerYPosition = player.transform.position.y;
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
