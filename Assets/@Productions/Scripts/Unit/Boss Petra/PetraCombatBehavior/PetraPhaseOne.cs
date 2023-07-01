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
