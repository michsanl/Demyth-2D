using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PetraAbilityTester : PetraAbilityCollection
{
    [EnumToggleButtons]
    public Ability loopAbility;
    public enum Ability
    { UpCharge, DownCharge, HorizontalCharge, SpinAttack, ChargeAttack, BasicSlam, JumpSlam }
    
    public bool EnableAbilityTester 
    {
        get => enableAbilityTester;
        set { enableAbilityTester = value; }
    }
    private bool enableAbilityTester;
    

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
            case Ability.UpCharge:
                StartCoroutine(PlayAbilityUpCharge());
                break;
            case Ability.DownCharge:
                StartCoroutine(PlayAbilityDownCharge());
                break;
            case Ability.HorizontalCharge:
                StartCoroutine(PlayAbilityHorizontalCharge());
                break;
            case Ability.SpinAttack:
                StartCoroutine(PlayAbilitySpinAttack());
                break;
            case Ability.ChargeAttack:
                StartCoroutine(PlayAbilityChargeAttack());
                break;
            case Ability.BasicSlam:
                StartCoroutine(PlayAbilityBasicSlam());
                break;
            case Ability.JumpSlam:
                StartCoroutine(PlayAbilityJumpSlam());
                break;
            default:
                break;
        }
    }
}
