using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSri_AbilityTester : BossSri_Base
{
    private int count;

    public Ability ability;
    public enum Ability
    {
        NailAOE,
        NailSummon,
        FireBall,
        SpinClaw,
        Slash,
    }

    protected override void OnActivate()
    {
        base.OnActivate();
    }

    protected override void OnTick()
    {
        base.OnTick();
        
        if (isBusy)
            return;

        HandlePlayAbility();
    }

    private void HandlePlayAbility()
    {
        switch (ability)
        {
            case Ability.NailAOE:
                StartCoroutine(PlayNailAOE());
                break;
            case Ability.FireBall:
                StartCoroutine(PlayFireBall());
                break;
            case Ability.NailSummon:
                StartCoroutine(PlayNailSummon1());
                break;
            case Ability.SpinClaw:
                StartCoroutine(PlaySpinClaw());
                break;
            case Ability.Slash:
                PlayMovingAbility();
                break;
            default:
                break;
        }
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
}
