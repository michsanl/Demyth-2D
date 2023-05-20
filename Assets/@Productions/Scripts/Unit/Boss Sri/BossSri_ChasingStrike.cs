using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;

public class BossSri_ChasingStrike : SceneService
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
        float targetPosition = Mathf.Round(Context.Player.transform.position.x);
        
        if (targetPosition == transform.position.x)
        {
            if (IsPlayerAbove())
            {
                StartCoroutine(PlayUpSlash());
                return;
            }
            if (!IsPlayerAbove())
            {
                StartCoroutine(PlayDownSlash());
                return;
            }
        }

        if (IsPlayerToRight())
        {
            StartCoroutine(PlayRightSlash(targetPosition));
            return;
        } 
        if (IsPlayerToLeft())
        {
            StartCoroutine(PlayLeftSlash(targetPosition));
            return;
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

    private bool IsPlayerToLeft()
    {
        return transform.position.x > Context.Player.transform.position.x;
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

// public class BossSriVer1 : SceneService
// {
    
//     [SerializeField] private float startDelay;
//     [SerializeField] private float exitDelay;
//     [SerializeField] private float moveDuration;

//     [SerializeField] private Animator animator;

//     private LookOrientation lookOrientation;

//     private bool isBusy;
//     private bool isIntroPlayed;

//     private void Awake()
//     {
//         lookOrientation = GetComponent<LookOrientation>();
//     }

//     private void Start()
//     {
//         StartCoroutine(PlayIntroAnimation());
//     }

//     void Update()
//     {
//         if (!isIntroPlayed)
//             return;
//         if (isBusy)
//             return;
        
//         HandleAction();
//     }

//     private void HandleAction()
//     {
//         if (IsPlayerAbove())
//         {
//             if (IsPlayerToRight())
//             {
//                 StartCoroutine(PlayUpRightSlash());
//             } else
//             {
//                 StartCoroutine(PlayUpLeftSlash());
//             }
//         } else
//         {
//             if (IsPlayerToRight())
//             {
//                 StartCoroutine(PlayDownRightSlash());
//             } else
//             {
//                 StartCoroutine(PlayDownLeftSlash());
//             }
//         }
//     }

//     private bool IsPlayerAbove()
//     {
//         return transform.position.y < Context.Player.transform.position.y;
//     }

//     private bool IsPlayerToRight()
//     {
//         return transform.position.x < Context.Player.transform.position.x;
//     }

//     private bool IsPlayerToLeft()
//     {
//         return transform.position.x > Context.Player.transform.position.x;
//     }


//     private IEnumerator PlayIntroAnimation()
//     {
//         isBusy = true;

//         animator.Play("Intro");
//         yield return Helper.GetWaitForSeconds(4.1f);
//         isIntroPlayed = true;

//         isBusy = false;
//     }

// #region Type1
//     private IEnumerator PlayUpRightSlash()
//     {
//         isBusy = true;

//         lookOrientation.SetFacingDirection(Vector2.right);
//         animator.Play("Up_Right_Slash");
//         yield return Helper.GetWaitForSeconds(.7f);
//         transform.DOMoveY(transform.position.y + 3f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);
//         transform.DOMoveX(transform.position.x + 6f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);

//         isBusy = false;
//     }

//     private IEnumerator PlayUpLeftSlash()
//     {
//         isBusy = true;

//         lookOrientation.SetFacingDirection(Vector2.left);
//         animator.Play("Up_Right_Slash");
//         yield return Helper.GetWaitForSeconds(.7f);
//         transform.DOMoveY(transform.position.y + 3f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);
//         transform.DOMoveX(transform.position.x - 6f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);

//         isBusy = false;
//     }

//     private IEnumerator PlayDownLeftSlash()
//     {
//         isBusy = true;

//         lookOrientation.SetFacingDirection(Vector2.left);
//         animator.Play("Down_Left_Slash");
//         yield return Helper.GetWaitForSeconds(.7f);
//         transform.DOMoveY(transform.position.y - 3f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);
//         transform.DOMoveX(transform.position.x - 6f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);

//         isBusy = false;
//     }

//     private IEnumerator PlayDownRightSlash()
//     {
//         isBusy = true;

//         lookOrientation.SetFacingDirection(Vector2.right);
//         animator.Play("Down_Left_Slash");
//         yield return Helper.GetWaitForSeconds(.7f);
//         transform.DOMoveY(transform.position.y - 3f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);
//         transform.DOMoveX(transform.position.x + 6f, .5f).SetEase(Ease.OutExpo);
//         yield return Helper.GetWaitForSeconds(1f);

//         isBusy = false;
//     }
// #endregion
// }
