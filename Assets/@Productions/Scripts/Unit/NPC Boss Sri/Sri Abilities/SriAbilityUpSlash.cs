using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;

public class SriAbilityUpSlash : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject upSlashCollider;
    
    private int topArenaBorder = 2;
    private int bottomArenaBorder = -4;
    private int UP_SLASH = Animator.StringToHash("Up_Slash");

    public IEnumerator UpSlash()
    {
        var audioManager = Context.AudioManager;
        var playerYPosition = Context.Player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(UP_SLASH);
        audioManager.PlaySound(audioManager.SriAudioSource.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        upSlashCollider.SetActive(true);
        yield return transform.DOMoveY(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        upSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
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
