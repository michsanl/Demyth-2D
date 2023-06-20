using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class SriPhaseTwo : SriAbilityCollection
{
    public bool ActivateSecondPhase
    {
        get => activateSecondPhase;
        set 
        {
            activateSecondPhase = value;
            if (activateSecondPhase == true)
                Debug.Log("Second phase is active");
        }
    }
    
    [SerializeField] private  bool activateSecondPhase;

    protected override void OnTick()
    {
        base.OnTick();

        HandleAction();
    }

    private void HandleAction()
    {
        if (!activateSecondPhase)
            return;
        if (isBusy)
            return;

        SetFacingDirection();

        PlayTeleportAtRandomChance();

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
            PlayAbilityTeleport();
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
