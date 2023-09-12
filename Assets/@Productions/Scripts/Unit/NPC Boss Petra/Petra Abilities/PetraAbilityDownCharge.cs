using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityDownCharge : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private LayerMask moveBlockLayerMask;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject downChargeCollider;
    
    private int DOWN_CHARGE = Animator.StringToHash("Down_charge");

    public IEnumerator DownCharge()
    {
        var targetPosition = Context.Player.transform.position.y;
        targetPosition = SetPositionToBehindPlayer(targetPosition);
        targetPosition = ClampToMoveBlocker(targetPosition);
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(DOWN_CHARGE);
        var audioManager = Context.AudioManager;
        audioManager.PlaySound(audioManager.PetraAudioSource.RunCharge);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        downChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        downChargeCollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwingDuration);
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
