using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class BossSri_WaitToStrike : SceneService
{
    [SerializeField] private float startDelay;
    [SerializeField] private float exitDelay;
    [SerializeField] private float moveDuration;

    [SerializeField] private Animator animator;

    private LookOrientation lookOrientation;

    private bool isBusy;
    private bool isIntroPlayed;

    private void Awake()
    {
        lookOrientation = GetComponent<LookOrientation>();
    }

    private void Start()
    {
        StartCoroutine(PlayIntroAnimation());
    }

    void Update()
    {
        if (!isIntroPlayed)
            return;
        if (isBusy)
            return;
        
        HandleAction();
    }

    private void HandleAction()
    {   
        if (IsPlayerAtSamePosY())
        {
            if (IsPlayerAtSamePosX())
                return;

            if (IsPlayerToRight())
            {
                StartCoroutine(PlayRightSlash(transform.position.x + 6f));
                return;
            } else
            {
                StartCoroutine(PlayLeftSlash(transform.position.x - 6f));
                return;
            }
        }

        if (IsPlayerAtSamePosX())
        {
            if (IsPlayerAtSamePosY())
                return;

            if (IsPlayerAbove())
            {
                StartCoroutine(PlayUpSlash());
                return;
            } else
            {
                StartCoroutine(PlayDownSlash());
                return;
            }
        }
    }

    private bool IsPlayerAbove()
    {
        return transform.position.y < Context.Player.transform.position.y;
    }

    private bool IsPlayerBelow()
    {
        return transform.position.y > Context.Player.transform.position.y;
    }

    private bool IsPlayerToRight()
    {
        return transform.position.x < Context.Player.transform.position.x;
    }

    private bool IsPlayerToLeft()
    {
        return transform.position.x > Context.Player.transform.position.x;
    }

    private bool IsPlayerAtSamePosX()
    {
        return transform.position.x == Context.Player.transform.position.x;
    }

    private bool IsPlayerAtSamePosY()
    {
        return transform.position.y == Context.Player.transform.position.y;
    }

    private IEnumerator PlayIntroAnimation()
    {
        isBusy = true;

        animator.Play("Intro");
        yield return Helper.GetWaitForSeconds(4.1f);
        isIntroPlayed = true;

        isBusy = false;
    }

    private IEnumerator PlayRightSlash(float targetPosition)
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.right);
        animator.Play("Horizontal_Slash_Fast");
        yield return Helper.GetWaitForSeconds(startDelay);
        transform.DOMoveX(targetPosition, moveDuration).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(exitDelay);

        isBusy = false;
    }

    private IEnumerator PlayLeftSlash(float targetPosition)
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.left);
        animator.Play("Horizontal_Slash_Fast");
        yield return Helper.GetWaitForSeconds(startDelay);
        transform.DOMoveX(targetPosition, moveDuration).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(exitDelay);

        isBusy = false;
    }

    private IEnumerator PlayUpSlash()
    {
        isBusy = true;

        animator.Play("Up_Slash_Normal");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(2, .3f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(.5f);

        isBusy = false;
    }

    private IEnumerator PlayDownSlash()
    {
        isBusy = true;

        animator.Play("Down_Slash_Normal");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(-4, .3f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(.5f);

        isBusy = false;
    }
}
