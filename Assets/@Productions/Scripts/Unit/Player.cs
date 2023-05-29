using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomTools.Core;
using UnityEngine.InputSystem;
using UISystem;

public class Player : CoreBehaviour
{
    [SerializeField] private GameObject senterGameObject;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;

    private PlayerInputActions playerInputActions;
    private MovementController movementController;
    private LookOrientation lookOrientation;
    private Health health;
    private bool isBusy;
    private bool isMoving;
    private bool isSenterEnabled;
    private bool isSenterUnlocked = true;
    private bool isHealthPotionOnCooldown;
    private bool isHealthPotionUnlocked = true;
    private Vector2 playerDir;
    private Vector2 lastPlayerDir = Vector2.down;
    public Vector2 lastPlayerPosition;

    public bool isGamePaused;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += OnHealthPotionPerformed;
        playerInputActions.Player.Pause.performed += OnPausePerformed;

        playerInputActions.Player.Enable();

        movementController = GetComponent<MovementController>();
        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        HandlePlayerAction();
    }

    private void HandlePlayerAction()
    {
        if (isGamePaused)
            return;
        if (isBusy)
            return;

        playerDir = playerInputActions.Player.MovePassThrough.ReadValue<Vector2>();

        if (playerDir == Vector2.zero)
            return;
        if (IsDirectionDiagonal(playerDir))
            return;
        
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, movementBlockerLayerMask, out Interactable interactable))
        {
            if (interactable != null)
            {
                StartCoroutine(HandleInteract(interactable));
                lastPlayerPosition = (Vector2)transform.position + playerDir;
            }
        } 
        else
        {
            StartCoroutine(HandleMovement());
        }

        lastPlayerDir = playerDir;
    }

    private bool IsDirectionDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }

    private IEnumerator HandleMovement()
    {
        isBusy = true;

        movementController.Move(playerDir, moveDuration);
        animator.SetTrigger("Dash");
        yield return Helper.GetWaitForSeconds(actionDelay);

        isBusy = false;
    }

    private IEnumerator HandleInteract(Interactable interactable)
    {
        isBusy = true;
        
        switch (interactable.interactableType)
        {
            case InteractableType.Push:
                animator.SetTrigger("Attack");
                break;
            case InteractableType.Damage:
                animator.SetTrigger("Attack");
                break;
            default:
                break;
        }

        interactable.Interact(playerDir);

        yield return Helper.GetWaitForSeconds(actionDelay);

        isBusy = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        StartCoroutine(KnockedBack());
    }

    public IEnumerator KnockedBack()
    {
        isBusy = true;

        Vector2 oppositeDir = GetOppositeDir(lastPlayerDir);

        if (!Helper.CheckTargetDirection(transform.position, oppositeDir, movementBlockerLayerMask, out Interactable interactable))
        {
            movementController.Move(oppositeDir, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        } else
        {
            movementController.Move(lastPlayerDir, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        }

        isBusy = false;
    }

    public Vector2 GetOppositeDir(Vector2 vector)
    {
        return new Vector2(vector.x * -1, vector.y * -1);
    }
    
    private void OnSenterPerformed(InputAction.CallbackContext context)
    {
        if (!isSenterUnlocked)
            return;

        ToggleSenter();

        Context.HUDUI.SetActiveSenterImage(isSenterEnabled);
    }

    private void ToggleSenter()
    {
        isSenterEnabled = !isSenterEnabled;
        senterGameObject.SetActive(isSenterEnabled);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        ToggleGamePause();

        Context.UI.Toggle<PauseUI>();
    }

    public void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    private void OnHealthPotionPerformed(InputAction.CallbackContext context)
    {
        if (!isHealthPotionUnlocked)
            return;
        if (isHealthPotionOnCooldown)
            return;

        StartCoroutine(HealSelf());
    }

    private IEnumerator HealSelf()
    {
        isHealthPotionOnCooldown = true;

        health.Heal(1);

        yield return Helper.GetWaitForSeconds(actionDelay);

        isHealthPotionOnCooldown = false;
    }

}
