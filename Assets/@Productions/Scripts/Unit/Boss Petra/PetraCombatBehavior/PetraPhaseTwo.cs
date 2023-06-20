using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class PetraPhaseTwo : PetraAbilityCollection
{
    public bool EnableSecondPhase
    {
        get => enableSecondPhase;
        set 
        {
            enableSecondPhase = value;
            if (enableSecondPhase == true)
                Debug.Log("Second phase is active");
        }
    }

    private IEnumerator[] playerNearbyMovesetArray;

    private bool enableSecondPhase;

    protected override void OnActivate()
    {
        playerNearbyMovesetArray = new IEnumerator[] { PlayAbilitySpinAttack(), PlayAbilityChargeAttack() };
    }

    protected override void OnTick()
    {
        if (!enableSecondPhase)
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
            Vector2 playerLastPosition = Context.Player.LastMoveTargetPosition;
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityJumpSlam(playerLastPosition));
            } else
            {;
                StartCoroutine(PlayAbilityBasicSlam(playerLastPosition));
            }
            return;
        }
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
