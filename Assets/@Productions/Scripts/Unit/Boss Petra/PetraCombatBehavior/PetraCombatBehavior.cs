using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class PetraCombatBehavior : PetraAbilityCollection
{
    [SerializeField] private bool isCombatMode;
    private bool isFirstPhase;
    private bool isSecondPhase;

    protected override void OnActivate()
    {
        base.OnActivate();

        health.OnAfterTakeDamage += Health_OnAfterTakeDamage;
        ActivatePhaseOne();
    }

    protected override void OnTick()
    {
        if (!isCombatMode)
            return;
        if (isFirstPhase)
            FirstPhaseRoutine();
        if (isSecondPhase)
            SecondPhaseRoutine();
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
            PlayHorizontalAbility();
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalAbility();
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
            PlayHorizontalAbility();
            return;
        }
        if (IsPlayerAtSamePosX())
        {
            PlayVerticalAbility();
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

    private void Health_OnAfterTakeDamage()
    {
        if (health.CurrentHP <= 10)
        {
            ActivatePhaseTwo();
        }
    }

    private void ActivatePhaseOne()
    {
        isFirstPhase = true;
        isSecondPhase = false;
    }

    private void ActivatePhaseTwo()
    {
        isFirstPhase = false;
        isSecondPhase = true;
    }

    private void TurnOffCombatMode()
    {
        isFirstPhase = false;
        isSecondPhase = false;
    }

    private int GetRandomIndexFromList(List<IEnumerator> abilityList)
    {
        return UnityEngine.Random.Range(0, abilityList.Count);
    }

    private void PlayVerticalAbility()
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

    private void PlayHorizontalAbility()
    {
        if (IsPlayerToRight())
        {
            StartCoroutine(PlayAbilityHorizontalCharge());
            return;
        } else
        {
            StartCoroutine(PlayAbilityHorizontalCharge());
            return;
        }
    }
}
