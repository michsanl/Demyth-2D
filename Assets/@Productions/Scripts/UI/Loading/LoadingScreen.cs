using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class LoadingScreen : UIPageView
{
    public bool IsPlaying { get; private set; }

    [SerializeField]
    private Animator loadingAnimator;
    [SerializeField]
    private float additionalTimeForAnimator = 2f;

    private const string LOAD_OPEN = "Load Open";
    private const string LOAD_CLOSE = "Load Close";

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        // Start animasi open
        StartCoroutine(PlayAnimation(LOAD_OPEN));
    }

    protected override void OnClosed()
    {
        // Start animasi close
        StartCoroutine(PlayAnimation(LOAD_CLOSE));
    }

    public void ShowLoading(bool condition)
    {
        gameObject.SetActive(condition);
    }

    private IEnumerator PlayAnimation(string state)
    {
        IsPlaying = true;        
        loadingAnimator.Play(state);
        
        yield return new WaitForSeconds(additionalTimeForAnimator);
        IsPlaying = false;
    }
}
