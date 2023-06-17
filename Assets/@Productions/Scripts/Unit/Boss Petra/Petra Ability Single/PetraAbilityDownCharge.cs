using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityDownCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject downChargeCollider;
    
    private int DOWN_CHARGE = Animator.StringToHash("Down_charge");

    public IEnumerator DownCharge(float targetPosition)
    {
        var finalTargetPosition = Mathf.Clamp(Mathf.RoundToInt(targetPosition), -4, 2);

        animator.Play(DOWN_CHARGE);
        // audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        downChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        downChargeCollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
