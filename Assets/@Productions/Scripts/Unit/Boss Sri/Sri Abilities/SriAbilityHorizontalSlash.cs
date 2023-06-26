using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;

public class SriAbilityHorizontalSlash : SceneService
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
    [SerializeField] private GameObject horizontalSlashCollider;
    
    protected int HORIZONTAL_SLASH = Animator.StringToHash("Horizontal_Slash");

    public IEnumerator HorizontalSlash(Player player)
    {
        var audioManager = Context.AudioManager;
        float playerXPosition = player.transform.position.x;
        float targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerXPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        animator.Play(HORIZONTAL_SLASH);
        audioManager.PlayClipAtPoint(audioManager.SriAudioSource.HorizontalSlash, transform.position);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        horizontalSlashCollider.SetActive(true);
        yield return transform.DOMoveX(finalTargetPosition, swingDuration).SetEase(animationCurve).WaitForCompletion();
        horizontalSlashCollider.SetActive(false);
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
