using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;
using Sirenix.OdinInspector;

public class BossSri_Base : SceneService
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected LookOrientation lookOrientation;
    [SerializeField] private AudioClipSriSO audioClipSriSO;

    private AudioManager audioManager;

    protected bool isBusy;
    protected bool isIntroPlayed;

    protected override void OnActivate()
    {
        audioManager = Context.AudioManager;
    }

#region PlayerToBossPositionInfo
    protected bool IsPlayerAbove()
    {
        return transform.position.y < Context.Player.transform.position.y;
    }

    protected bool IsPlayerBelow()
    {
        return transform.position.y > Context.Player.transform.position.y;
    }

    protected bool IsPlayerToRight()
    {
        return transform.position.x < Context.Player.transform.position.x;
    }

    protected bool IsPlayerToLeft()
    {
        return transform.position.x > Context.Player.transform.position.x;
    }

    protected bool IsPlayerAtSamePosX()
    {
        return transform.position.x == Context.Player.transform.position.x;
    }

    protected bool IsPlayerAtSamePosY()
    {
        return transform.position.y == Context.Player.transform.position.y;
    }
#endregion

    protected IEnumerator PlayIntro()
    {
        isBusy = true;

        animator.Play("Intro");
        yield return Helper.GetWaitForSeconds(4.1f);
        isIntroPlayed = true;

        isBusy = false;
    }

    protected IEnumerator PlayRightSlash(float targetPosition)
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.right);

        float frontSwing = .833f;
        float swing = .233f;
        float backSwing = 1.267f;

        animator.Play("Horizontal Slash");
        audioManager.PlaySound(audioClipSriSO.HorizontalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        yield return transform.DOMoveX(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayLeftSlash(float targetPosition)
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.left);
        
        float frontSwing = .833f;
        float swing = .233f;
        float backSwing = 1.267f;

        animator.Play("Horizontal Slash");
        audioManager.PlaySound(audioClipSriSO.HorizontalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        yield return transform.DOMoveX(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayUpSlash(float targetPosition)
    {
        isBusy = true;

        float frontSwing = .6f;
        float swing = .233f;
        float backSwing = 1.1f;

        animator.Play("Up Slash");
        audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        yield return transform.DOMoveY(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayDownSlash(float targetPosition)
    {
        isBusy = true;

        float frontSwing = .6f;
        float swing = .233f;
        float backSwing = 1.1f;

        animator.Play("Down Slash");
        audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        yield return transform.DOMoveY(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

}
