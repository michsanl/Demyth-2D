using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class PetraCombatBehavior : PetraAbilityCollection
{
    [SerializeField] private bool activateCombatMode;
    [EnumToggleButtons] public CombatMode SelectCombatMode;
    [Space]
    [EnumToggleButtons] public Ability LoopAbility;
    
    public enum Ability 
    { UpCharge, DownCharge, HorizontalCharge, SpinAttack, ChargeAttack, BasicSlam, JumpSlam }
    public enum CombatMode 
    { FirstPhase, SecondPhase, AbilityLoop }

    protected override void OnTick()
    {
        if (!activateCombatMode)
            return;
        
        switch (SelectCombatMode)
        {
            case CombatMode.FirstPhase:
                FirstPhaseRoutine();
                break;
            case CombatMode.SecondPhase:
                SecondPhaseRoutine();
                break;
            case CombatMode.AbilityLoop:
                AbilityLoopRoutine();
                break;
        }
    }

    private void FirstPhaseRoutine()
    {
        if (isBusy)
            return;

        SetFacingDirection();

        if (IsPlayerNearby())
        {
            StartCoroutine(PlayAbilitySpinAttack());
            return;
        }


        if (IsPlayerAtSamePosY())
        {
            StartCoroutine(PlayAbilityHorizontalCharge());
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalCharge();
            return;
        }


        if (!IsPlayerNearby())
        {
            StartCoroutine(PlayAbilityBasicSlam());
            return;
        }
        
    }

    private void SecondPhaseRoutine()
    {
        if (isBusy)
            return;

        SetFacingDirection();

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilitySpinAttack());
            } else
            {
                StartCoroutine(PlayAbilityChargeAttack());
            }
            return;
        }


        if (IsPlayerAtSamePosY())
        {
            StartCoroutine(PlayAbilityHorizontalCharge());
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalCharge();
            return;
        }


        if (!IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityJumpSlam());
            } else
            {;
                StartCoroutine(PlayAbilityBasicSlam());
            }
            return;
        }
    }

    private void AbilityLoopRoutine()
    {
        if (isBusy)
            return;

        SetFacingDirection();

        switch (LoopAbility)
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

    private void PlayVerticalCharge()
    {
        if (IsPlayerAbove())
        {
            StartCoroutine(PlayAbilityUpCharge());
            return;
        } else
        {
            StartCoroutine(PlayAbilityDownCharge());
            return;
        }
    }
}
