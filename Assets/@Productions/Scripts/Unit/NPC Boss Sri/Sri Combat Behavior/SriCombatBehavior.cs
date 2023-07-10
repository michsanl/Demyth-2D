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
    VerticalNailWave }
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

    private IEnumerator TeleportMultipleTimes()
    {
        isBusy = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(abilityTeleport.Teleport());            
            SetFacingDirection();
        }

        if (abilityHorizontalNailWave.GetNailGameObject() != null)
        {
            Destroy(abilityHorizontalNailWave.GetNailGameObject());
        }

        isBusy = false;
    }

    protected override void OnTick()
    {
        if (!activateCombatMode)
            return;
        if (healthDecreaseCount >= 3)
        {
            if (activeCoroutine != null)
                StopCoroutine(activeCoroutine);
                
            StartCoroutine(TeleportMultipleTimes());
            healthDecreaseCount = 0;
            return;
        }
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
            case CombatMode.NewSecondPhase:
                StartCoroutine(NewSecondPhaseRoutine());
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

    private IEnumerator NewSecondPhaseRoutine()
    {
        isBusy = true;

        int teleportCount =  UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(PlayAbilityTeleport());
            SetFacingDirection();
        }

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(PlayAbilityHorizontalNailWave());
        }
        else
        {
            yield return StartCoroutine(PlayAbilityVerticalNailWave());
        }

        yield return StartCoroutine(PlayAbilityTeleport());
        SetFacingDirection();
        FirstPhaseRoutine();
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
            case Ability.HorizontalNailWave:
                StartCoroutine(PlayAbilityHorizontalNailWave());
                break;
            case Ability.VerticalNailWave:
                StartCoroutine(PlayAbilityVerticalNailWave());
                break;
            default:
                break;
        }
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
