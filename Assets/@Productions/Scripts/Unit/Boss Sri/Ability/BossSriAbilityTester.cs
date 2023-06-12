using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class BossSriAbilityTester : BossSriAbility
{
    [EnumToggleButtons]
    public Ability loopAbility;
    public enum Ability
    { NailAOE, NailSummon, FireBall, SpinClaw, SlashInCircle, }

    public bool ActivateTester
    {
        get => activateTester;
        set 
        {
            activateTester = value;
        }
    }

    private bool activateTester;
    private int count;

    protected override void OnTick()
    {
        base.OnTick();

        if (!activateTester)
            return;
        if (isBusy)
            return;

        HandlePlayAbility();
    }

    private void HandlePlayAbility()
    {
        switch (loopAbility)
        {
            case Ability.NailAOE:
                StartCoroutine(PlayNailAOE());
                break;
            case Ability.FireBall:
                StartCoroutine(PlayFireBall());
                break;
            case Ability.NailSummon:
                StartCoroutine(PlayNailSummon(groundNailSingle));
                break;
            case Ability.SpinClaw:
                StartCoroutine(PlaySpinClaw());
                break;
            case Ability.SlashInCircle:
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
