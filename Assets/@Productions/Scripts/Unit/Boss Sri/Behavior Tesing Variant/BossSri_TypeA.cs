using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSri_TypeA : BossSri_Base
{
    [SerializeField] private bool isCombatMode;

    private Action[] movementAction;

    // private void Start() 
    // {
    //     movementAction = new Action[] {HorizontalMovement, VerticalMovement};
    // }

    // private void Update() 
    // {
    //     HandleAction();
    // }

    protected override void OnActivate()
    {
        base.OnActivate();

        movementAction = new Action[] {HorizontalMovement, VerticalMovement};
    }

    protected override void OnTick()
    {
        base.OnTick();

        HandleAction();
    }

    private void HandleAction()
    {
        if (!isCombatMode)
            return;

        if (isBusy)
            return;

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

        if (isMoving)
            return;

        HandleMovement();
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
        int i = UnityEngine.Random.Range(0,2);
        movementAction[i]?.Invoke();
    }

    private void HorizontalMovement()
    {
        SetFacingDirection();

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
        SetFacingDirection();

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
