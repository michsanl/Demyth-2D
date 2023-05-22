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
    protected bool isMoving;
    protected bool isIntroPlayed;

    protected int INTRO = Animator.StringToHash("Intro");
    protected int UP_SLASH = Animator.StringToHash("Up_Slash");
    protected int DOWN_SLASH = Animator.StringToHash("Down_Slash");
    protected int HORIZONTAL_SLASH = Animator.StringToHash("Horizontal_Slash");
    protected int NAIL_AOE = Animator.StringToHash("Nail_AOE");
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");
    protected int FIRE_BALL = Animator.StringToHash("Fire_Ball");
    protected int NAIL_SUMMON_1 = Animator.StringToHash("Nail_Summon_1");

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

    protected IEnumerator PlayMove(Vector2 direction)
    {
        isMoving = true;

        Vector2 moveTargetPosition = (Vector2)transform.position + direction;
        float moveDuration = .25f;
        float actionDelay = 1f;

        transform.DOMove(moveTargetPosition, moveDuration).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(actionDelay);

        isMoving = false;
    }


    protected IEnumerator PlayIntro()
    {
        isBusy = true;

        animator.Play(INTRO);
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

        animator.Play(HORIZONTAL_SLASH);
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

        animator.Play(HORIZONTAL_SLASH);
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

        animator.Play(UP_SLASH);
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

        animator.Play(DOWN_SLASH);
        audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        yield return transform.DOMoveY(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayNailAOE()
    {
        isBusy = true;

        float animationDuration = 4.1f;
        animator.Play(NAIL_AOE);
        audioManager.PlaySound(audioClipSriSO.NailAOE);
        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

    protected IEnumerator PlaySpinClaw()
    {
        isBusy = true;

        float animationDuration = 2.5f;
        animator.Play(SPIN_CLAW);
        audioManager.PlaySound(audioClipSriSO.SpinClaw);
        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

    protected IEnumerator PlayFireBall()
    {
        isBusy = true;

        float animationDuration = 2.0667f;
        animator.Play(FIRE_BALL);
        // audioManager.PlaySound(audioClipSriSO.Fireball);
        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

    protected IEnumerator PlayNailSummon1()
    {
        isBusy = true;

        float animationDuration = 1.8f;
        animator.Play(NAIL_SUMMON_1);
        audioManager.PlaySound(audioClipSriSO.NailSummon);
        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

}
