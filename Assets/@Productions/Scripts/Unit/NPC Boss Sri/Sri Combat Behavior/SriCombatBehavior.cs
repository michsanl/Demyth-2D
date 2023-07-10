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
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport }
    public enum CombatMode 
    { FirstPhase, SecondPhase, NewSecondPhase, AbilityLoop }

    private int healthDecreaseCount;

    protected override void OnActivate()
    {
        base.OnActivate();

        health.OnTakeDamage += Health_OnTakeDamage;
    }

    private void Health_OnTakeDamage()
    {
        healthDecreaseCount++;
    }

    protected override void OnTick()
    {
        if (!activateCombatMode)
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
            case CombatMode.NewSecondPhase:
                NewSecondPhaseRoutine();
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
        if (isBusy)
            return;

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

    private void NewSecondPhaseRoutine()
    {
        if (healthDecreaseCount >= 3)
        {
            StopCurrentAbility();

            primaryActiveCoroutine = StartCoroutine(PlayAbilityWaveOutNailWave());

            healthDecreaseCount = 0;
            return;
        }
        if (isBusy)
            return;

        healthDecreaseCount = 0;
        primaryActiveCoroutine = StartCoroutine(TeleportIntoNailWaveVariant());
    }

    private void AbilityLoopRoutine()
    {
        if (isBusy)
            return;

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
            case Ability.HorizontalNailWave:
                StartCoroutine(PlayAbilityHorizontalNailWave());
                break;
            case Ability.VerticalNailWave:
                StartCoroutine(PlayAbilityVerticalNailWave());
                break;
            case Ability.WaveOutNailWave:
                StartCoroutine(PlayAbilityWaveOutNailWave());
                break;
            case Ability.Teleport:
                StartCoroutine(PlayAbilityTeleport());
                break;
            default:
                break;
        }
    }
    
    private void StopCurrentAbility()
    {
        if (primaryActiveCoroutine != null)
            StopCoroutine(primaryActiveCoroutine);
        if (secondaryActiveCoroutine != null)
            StopCoroutine(secondaryActiveCoroutine);
    }

    protected IEnumerator TeleportIntoNailWaveVariant()
    {
        isBusy = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return secondaryActiveCoroutine = StartCoroutine(abilityTeleport.Teleport());            
            SetFacingDirection();
        }

        int randomNumber = UnityEngine.Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                yield return secondaryActiveCoroutine = StartCoroutine(abilityHorizontalNailWave.PlayAbility());
                break;
            case 1:
                yield return secondaryActiveCoroutine = StartCoroutine(abilityVerticalNailWave.PlayAbility());
                break;
            case 2:
                yield return secondaryActiveCoroutine = StartCoroutine(abilityWaveOutNailWave.PlayAbility());
                break;
            default:
                break;
        }

        isBusy = false;
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
