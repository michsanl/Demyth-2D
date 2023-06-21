using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class SriCombatBehaviorPhaseOne : SriCombatBehaviorBase
{

    protected override void OnTick()
    {
        CombatBehaviorRoutine();
    }

    private void CombatBehaviorRoutine()
    {
        if (!isBehaviorActive)
            return;
        if (isBusy)
            return;

        SetFacingDirection();

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
            StartCoroutine(PlayAbilityNailSummon());
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

    private void PlayHorizontalAbility()
    {
        if (IsPlayerToRight())
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        } else
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        }
    }
}
