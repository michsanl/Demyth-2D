using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class PetraAbilityDownCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float _frontSwingDuration = 0.175f;
    [SerializeField] private float _swingDuration = 1.225f;
    [SerializeField] private float _backSwingDuration = 0.35f;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private GameObject downChargeCollider;
    
    private int DOWN_CHARGE = Animator.StringToHash("Down_charge");

    public IEnumerator DownCharge(Player player, Animator animator, AudioClip abilitySFX)
    {
        var targetPosition = player.transform.position.y;
        targetPosition = SetPositionToBehindPlayer(targetPosition);
        targetPosition = ClampToMoveBlocker(targetPosition);
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(DOWN_CHARGE);
        PlayAudio(abilitySFX);

        yield return Helper.GetWaitForSeconds(_frontSwingDuration);
        downChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, _swingDuration).SetEase(animationCurve).WaitForCompletion();
        downChargeCollider.SetActive(false);
        
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
