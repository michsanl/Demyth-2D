using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using CustomTools.Core;
using UnityEngine.Rendering.Universal;
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
    private bool isBusy = false;
    private bool isSenterEnabled;
    private bool isSenterUnlocked = true;
    private bool isHealthPotionUnlocked = true;
    private Vector3 playerDir;

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

        Vector2 inputVector = playerInputActions.Player.MovePassThrough.ReadValue<Vector2>();
        
        if (Math.Abs(inputVector.x) == Math.Abs(inputVector.y)) // biar gabisa gerak diagonal
            return;

        playerDir = inputVector;
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, movementBlockerLayerMask, out Interactable interactable))
        {
            if (interactable != null)
            {
                StartCoroutine(HandleInteract(interactable));
            }
        } 
        else
        {
            StartCoroutine(HandleMovement());
        }
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
    
    private void OnSenterPerformed(InputAction.CallbackContext context)
    {
        if (!isSenterUnlocked)
        {
            return;
        }
        isSenterEnabled = !isSenterEnabled;
        senterGameObject.SetActive(isSenterEnabled);
        Context.HUDUI.SetActiveSenterImage(isSenterEnabled);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        ToggleGamePause();
        Context.UI.Toggle<PauseUI>();
    }

    public void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnHealthPotionPerformed(InputAction.CallbackContext context)
    {
        if (!isHealthPotionUnlocked)
        {
            return;
        }
        if (isBusy)
        {
            return;
        }
        StartCoroutine(HealSelf());
    }

    private IEnumerator HealSelf()
    {
        isBusy = true;

        health.Heal(1);

        yield return Helper.GetWaitForSeconds(actionDelay);

        isBusy = false;
    }

}
