using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class BossSriFirstPhase : BossSriAbility
{
    public bool IsBehaviorActive
    {
        get => isBehaviorActive;
        set 
        {
            isBehaviorActive = value;

            if (isBehaviorActive == true)
            {
                Debug.Log("First phase is active");
            }
        }
    }


    private bool isBehaviorActive;
    private Action[] movementActionPoolArray;
    private List<IEnumerator> abilityPoolList;


    protected override void OnActivate()
    {
        base.OnActivate();

        movementActionPoolArray = new Action[] {HorizontalMovement, VerticalMovement};
        abilityPoolList = new List<IEnumerator>() {PlaySpinClaw(), PlayNailAOE()};
    }

    protected override void OnTick()
    {
        base.OnTick();

        HandleAction();
    }

    private void HandleAction()
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
                StartCoroutine(PlayNailAOE());
            } else
            {
                StartCoroutine(PlaySpinClaw());
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
            StartCoroutine(PlayNailSummon(groundNailSingle));
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
            StartCoroutine(PlayUpSlash(Context.Player.transform.position.y + 1f));
            return;
        } else
        {
            StartCoroutine(PlayDownSlash(Context.Player.transform.position.y - 1f));
            return;
        }
    }

    private void PlayHorizontalAbility()
    {
        if (IsPlayerToRight())
        {
            StartCoroutine(PlayRightSlash(Context.Player.transform.position.x + 1f));
            return;
        } else
        {
            StartCoroutine(PlayLeftSlash(Context.Player.transform.position.x - 1f));
            return;
        }
    }


    private void HandleMovement()
    {
        if (isMoving)
            return;

        int i = UnityEngine.Random.Range(0,2);
        movementActionPoolArray[i]?.Invoke();
    }

    private void HorizontalMovement()
    {
        //SetFacingDirection();

        if (IsPlayerToRight())
        {
            StartCoroutine(PlayMove(Vector2.right));
            return;
        }
        if (IsPlayerToLeft())
        {
            StartCoroutine(PlayMove(Vector2.left));
            return;
        }
    }

    private void VerticalMovement()
    {
        //SetFacingDirection();

        if (IsPlayerAbove())
        {
            StartCoroutine(PlayMove(Vector2.up));
            return;
        }
        if (IsPlayerBelow())
        {
            StartCoroutine(PlayMove(Vector2.down));
            return;
        }
    }

    private void SetFacingDirection()
    {
        if (IsPlayerToRight())
        {
            lookOrientation.SetFacingDirection(Vector2.right);
        }

        if (IsPlayerToLeft())
        {
            lookOrientation.SetFacingDirection(Vector2.left);
        }
    }
}
