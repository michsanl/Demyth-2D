using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityHorizontalCharge : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private int rightArenaBorder;
    [SerializeField] private int leftArenaBorder;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject horizontalChargeCollider;
    
    private int HORIZONTAL_CHARGE = Animator.StringToHash("Side_charge");

    public IEnumerator HorizontalCharge()
    {
        var playerXPosition = Context.Player.transform.position.x;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerXPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(HORIZONTAL_CHARGE);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        horizontalChargeCollider.SetActive(true);

        yield return transform.DOMoveX(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        horizontalChargeCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private float GetPositionWithIncrement(float playerXPosition)
    {
        return playerXPosition > transform.position.x ? playerXPosition + 2 : playerXPosition -2;
    }

    private float ClampValueToBattleArenaBorder(float value)
    {
        return Mathf.Clamp(value, leftArenaBorder, rightArenaBorder);
    }
}
