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
    [SerializeField] private int topArenaBorder;
    [SerializeField] private int bottomArenaBorder;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject downChargeCollider;
    
    private int DOWN_CHARGE = Animator.StringToHash("Down_charge");

    public IEnumerator DownCharge()
    {
        var playerYPosition = Context.Player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(DOWN_CHARGE);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        downChargeCollider.SetActive(true);

        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        downChargeCollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private float GetPositionWithIncrement(float playerYPosition)
    {
        return playerYPosition - 2;
    }

    private float ClampValueToBattleArenaBorder(float value)
    {
        return Mathf.Clamp(value, bottomArenaBorder, topArenaBorder);
    }
}
