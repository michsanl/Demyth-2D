using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class PetraPhaseOne : PetraAbilityCollection
{
    public bool EnableFirstPhase
    {
        get => enableFirstPhase;
        set 
        {
            enableFirstPhase = value;
            if (enableFirstPhase == true)
                Debug.Log("First phase is active");
        }
    }

    private bool enableFirstPhase;

    protected override void OnTick()
    {
        if (!enableFirstPhase)
            return;

        HandleAction();
    }

    private void HandleAction()
    {
        if (isBusy)
            return;

        SetFacingDirection();

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityBasicSlam());
            } 
            else
            {
                StartCoroutine(PlayAbilitySpinAttack());
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
            StartCoroutine(PlayAbilityJumpSlam(Context.Player.MoveTargetPosition));
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
            StartCoroutine(PlayAbilityUpCharge(Context.Player.transform.position.y + 1f));
            return;
        } else
        {
            StartCoroutine(PlayAbilityDownCharge(Context.Player.transform.position.y - 1f));
            return;
        }
    }

    private void PlayHorizontalAbility()
    {
        if (IsPlayerToRight())
        {
            StartCoroutine(PlayAbilityHorizontalCharge(Context.Player.transform.position.x + 1f));
            return;
        } else
        {
            StartCoroutine(PlayAbilityHorizontalCharge(Context.Player.transform.position.x - 1f));
            return;
        }
    }
}
