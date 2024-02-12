using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class PetraAbilityHorizontalCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float _frontSwingDuration = 0.175f;
    [SerializeField] private float _swingDuration = 1.225f;
    [SerializeField] private float _backSwingDuration = 0.35f;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject horizontalChargeCollider;
    
    private int HORIZONTAL_CHARGE = Animator.StringToHash("Side_charge");

    public IEnumerator HorizontalCharge(Player player, Animator animator, PetraClipSO petraClipSO)
    {
        var targetPosition = player.transform.position.x;
        if (targetPosition > transform.position.x)
        {
            targetPosition = SetPositionToPlayerRight(targetPosition);
            targetPosition = RightClampToMoveBlocker(targetPosition);
        }
        else
        {
            targetPosition = SetPositionToPlayerLeft(targetPosition);
            targetPosition = LeftClampToMoveBlocker(targetPosition);
        }
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(HORIZONTAL_CHARGE);
        Helper.PlaySFX(petraClipSO.RunCharge, petraClipSO.RunChargeVolume);

        yield return Helper.GetWaitForSeconds(_frontSwingDuration);
        horizontalChargeCollider.SetActive(true);

        yield return transform.DOMoveX(finalTargetPosition, _swingDuration).SetEase(animationCurve).WaitForCompletion();
        horizontalChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_backSwingDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    private float SetPositionToPlayerRight(float targetPosition)
    {
        return targetPosition + 2f;
    }

    private float SetPositionToPlayerLeft(float targetPosition)
    {
        return targetPosition - 2f;
    }

    private float RightClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, transform.position.x, GetClampMaxValue());
    }

    private float LeftClampToMoveBlocker(float value)
    {
        return Mathf.Clamp(value, GetClampMinValue(), transform.position.x);
    }

    private float GetClampMaxValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right, Vector2.right, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.x - .5f;
    }

    private float GetClampMinValue()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.left, Vector2.left, Mathf.Infinity, moveBlockLayerMask);
        return hit.point.x + .5f;
    }
}
