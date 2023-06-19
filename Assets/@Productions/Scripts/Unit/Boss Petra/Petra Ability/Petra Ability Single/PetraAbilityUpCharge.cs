using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class PetraAbilityUpCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upChargeCollider;
    
    private int UP_CHARGE = Animator.StringToHash("Up_charge");

    public IEnumerator UpCharge(float targetYPosition)
    {
        var finalTargetPosition = Mathf.Clamp(Mathf.RoundToInt(targetYPosition), -4, 3);

        animator.Play(UP_CHARGE);
        // audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        upChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        upChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
