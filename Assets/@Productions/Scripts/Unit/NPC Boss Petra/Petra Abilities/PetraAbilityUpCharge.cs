using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;
using MoreMountains.Tools;

public class PetraAbilityUpCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float _frontSwingDuration = 0.175f;
    [SerializeField] private float _swingDuration = 1.225f;
    [SerializeField] private float _backSwingDuration = 0.35f;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private GameObject upChargeCollider;
    
    private int UP_CHARGE = Animator.StringToHash("Up_charge");

    public IEnumerator UpCharge(Player player, Animator animator, PetraClipSO petraClipSO)
    {
        var targetPosition = player.transform.position.y;
        targetPosition = SetPositionToBehindPlayer(targetPosition);
        targetPosition = ClampToMoveBlocker(targetPosition);
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(UP_CHARGE);
        Helper.PlaySFX(petraClipSO.RunCharge, petraClipSO.RunChargeVolume);

        yield return Helper.GetWaitForSeconds(_frontSwingDuration);
        upChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, _swingDuration).SetEase(animationCurve).WaitForCompletion();
        upChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_backSwingDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    private float SetPositionToBehindPlayer(float playerYPosition)
    {
        return playerYPosition + 2f;
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
