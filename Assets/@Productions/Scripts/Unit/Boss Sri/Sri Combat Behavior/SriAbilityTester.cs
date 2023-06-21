using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SriAbilityTester : SriCombatBehaviorBase
{
    [EnumToggleButtons]
    public Ability loopAbility;
    public enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall }
    
    public bool EnableAbilityTester 
    {
        get => enableAbilityTester;
        set { enableAbilityTester = value; }
    }
    public bool enableAbilityTester;
    

    protected override void OnTick()
    {
        if (!enableAbilityTester)
            return;
        if (isBusy)
            return;

        SetFacingDirection();
        HandlePlayAbility();
    }

    private void HandlePlayAbility()
    {
        switch (loopAbility)
        {
            case Ability.UpSlash:
                StartCoroutine(PlayAbilityUpSlash());
                break;
            case Ability.DownSlash:
                StartCoroutine(PlayAbilityDownSlash());
                break;
            case Ability.HorizontalSlash:
                StartCoroutine(PlayAbilityHorizontalSlash());
                break;
            case Ability.SpinClaw:
                StartCoroutine(PlayAbilitySpinClaw());
                break;
            case Ability.NailAOE:
                StartCoroutine(PlayAbilityNailAOE());
                break;
            case Ability.NailSummon:
                StartCoroutine(PlayAbilityNailSummon());
                break;
            case Ability.FireBall:
                StartCoroutine(PlayAbilityFireBall());
                break;
            default:
                break;
        }
    }
}
