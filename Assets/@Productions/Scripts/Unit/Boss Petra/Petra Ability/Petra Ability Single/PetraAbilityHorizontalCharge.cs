using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityHorizontalCharge : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject rightChargeCollider;
    [SerializeField] private GameObject leftChargeCollider;
    
    private int HORIZONTAL_CHARGE = Animator.StringToHash("Side_charge");

    public IEnumerator HorizontalCharge(float targetXPosition)
    {
        var finalTargetPosition = Mathf.Clamp(Mathf.RoundToInt(targetXPosition), -6, 6);
        var collider = IsTargetPositionOnRight(targetXPosition) ? rightChargeCollider : leftChargeCollider;

        animator.Play(HORIZONTAL_CHARGE);
        // audioMHorizontal_CHARGEanager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        collider.SetActive(true);

        yield return transform.DOMoveX(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        collider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private bool IsTargetPositionOnRight(float targetXPosition)
    {
        return targetXPosition > transform.position.x;
    }
}
