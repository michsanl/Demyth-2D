using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class SriCombatBehaviorPhaseTwo : SriCombatBehaviorBase
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

    private void PlayTeleportAtRandomChance()
    {
        if (UnityEngine.Random.Range(0, 3) == 0)
        {
            StartCoroutine(PlayAbilityTeleport());
            SetFacingDirection();
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
