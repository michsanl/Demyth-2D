using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSri_Testing : BossSri_Base
{
    [SerializeField] private bool playMovingAbility;
    [SerializeField] private bool playStaticAbility;

    protected bool isOn;
    protected int count;

    protected override void OnActivate()
    {
        base.OnActivate();
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (playStaticAbility)
        {
            PlayStaticAbility();
            return;
        }

        if (playMovingAbility)
        {
            PlayMovingAbility();
            return;
        }

        // LoopPlayTwoAbility();
    }

    private void PlayStaticAbility()
    {
        if (count == 4)
        {
            count = 0;
        }

        switch (count)
        {
            case 0:
                StartCoroutine(PlayNailAOE());
                break;
            case 1:
                StartCoroutine(PlayFireBall());
                break;
            case 2:
                StartCoroutine(PlayNailSummon1());
                break;
            case 3:
                StartCoroutine(PlaySpinClaw());
                break;
            default:
                break;
        }

        count++;
    }

    private void PlayMovingAbility()
    {
        if (count == 4)
        {
            count = 0;
        }

        switch (count)
        {
            case 0:
                StartCoroutine(PlayRightSlash(3f));
                break;
            case 1:
                StartCoroutine(PlayUpSlash(1f));
                break;
            case 2:
                StartCoroutine(PlayLeftSlash(-3f));
                break;
            case 3:
                StartCoroutine(PlayDownSlash(-3f));
                break;
            default:
                break;
        }

        count++;
    }

    private void LoopPlayTwoAbility()
    {
        if (!isOn)
        {
            // StartCoroutine(PlayRightSlash(2f));
            StartCoroutine(PlayNailAOE());
            isOn = !isOn;
        }
        else
        {
            // StartCoroutine(PlayLeftSlash(-4f));
            StartCoroutine(PlaySpinClaw());
            isOn = !isOn;
        }
    }
}
