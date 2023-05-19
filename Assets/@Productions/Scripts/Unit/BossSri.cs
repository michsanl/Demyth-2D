using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class BossSri : SceneService
{
    [SerializeField] private Animator animator;

    private LookOrientation lookOrientation;

    private bool isBusy;
    private bool isIntroPlayed;

    protected override void OnInitialize()
    {
        lookOrientation = GetComponent<LookOrientation>();
    }

    protected override void OnActivate()
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
        if (IsPlayerAbove())
        {
            if (IsPlayerToRight())
            {
                StartCoroutine(PlayUpRightSlash());
            } else
            {
                StartCoroutine(PlayUpLeftSlash());
            }
        } else
        {
            if (IsPlayerToRight())
            {
                StartCoroutine(PlayDownRightSlash());
            } else
            {
                StartCoroutine(PlayDownLeftSlash());
            }
        }
    }

    private bool IsPlayerAbove()
    {
        return transform.position.y < Context.Player.transform.position.y;
    }

    private bool IsPlayerToRight()
    {
        return transform.position.x < Context.Player.transform.position.x;
    }


    private IEnumerator PlayIntroAnimation()
    {
        isBusy = true;

        animator.Play("Intro");
        yield return Helper.GetWaitForSeconds(4.1f);
        isIntroPlayed = true;

        isBusy = false;
    }

    private IEnumerator PlayUpRightSlash()
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.right);
        animator.Play("Up_Right_Slash");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(transform.position.y + 3f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);
        transform.DOMoveX(transform.position.x + 6f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);

        isBusy = false;
    }

    private IEnumerator PlayUpLeftSlash()
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.left);
        animator.Play("Up_Right_Slash");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(transform.position.y + 3f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);
        transform.DOMoveX(transform.position.x - 6f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);

        isBusy = false;
    }

    private IEnumerator PlayDownLeftSlash()
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.left);
        animator.Play("Down_Left_Slash");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(transform.position.y - 3f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);
        transform.DOMoveX(transform.position.x - 6f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);

        isBusy = false;
    }

    private IEnumerator PlayDownRightSlash()
    {
        isBusy = true;

        lookOrientation.SetFacingDirection(Vector2.right);
        animator.Play("Down_Left_Slash");
        yield return Helper.GetWaitForSeconds(.7f);
        transform.DOMoveY(transform.position.y - 3f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);
        transform.DOMoveX(transform.position.x + 6f, .5f).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(1f);

        isBusy = false;
    }
}
