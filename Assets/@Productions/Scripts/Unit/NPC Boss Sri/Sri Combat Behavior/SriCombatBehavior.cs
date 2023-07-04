using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class SriCombatBehavior : SriCombatBehaviorBase
{
    [SerializeField] private bool activateCombatMode;
    [EnumToggleButtons] public CombatMode SelectCombatMode;
    [EnumToggleButtons, Space] public Ability LoopAbility;

    public enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall }
    public enum CombatMode 
    { FirstPhase, SecondPhase, AbilityLoop }

    protected override void OnTick()
    {
        if (!activateCombatMode)
            return;
        if (isBusy)
            return;

        SetFacingDirection();
        
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
        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailAOE());
            } else
            {
                StartCoroutine(PlayAbilitySpinClaw());
            }
            return;
        }


        if (IsPlayerAtSamePosY())
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalAbility();
            return;
        }


        if (!IsPlayerNearby())
        {
            StartCoroutine(PlayAbilityNailSummon());
        }
        
    }

    private void SecondPhaseRoutine()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            StartCoroutine(PlayAbilityTeleport());
            SetFacingDirection();
            return;
        }

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailAOE());
            }
            else
            {
                StartCoroutine(PlayAbilitySpinClaw());
            }
            return;
        }

        if (IsPlayerAtSamePosY())
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalAbility();
            return;
        }


        if (!IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailSummon());
            }
            else
            {
                StartCoroutine(PlayAbilityFireBall());
            }
        }
    }

    private void AbilityLoopRoutine()
    {
        switch (LoopAbility)
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

    private int GetRandomIndexFromList(List<IEnumerator> abilityList)
    {
        return UnityEngine.Random.Range(0, abilityList.Count);
    }

    private void PlayVerticalAbility()
    {
        if (IsPlayerAbove())
        {
            StartCoroutine(PlayAbilityUpSlash());
            return;
        } else
        {
            StartCoroutine(PlayAbilityDownSlash());
            return;
        }
    }
}
