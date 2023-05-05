using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using CustomTools.Core;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class Player : CoreBehaviour
{
    [SerializeField] private Light2D senter;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;

    private PlayerInputActions playerInputActions;
    private MovementController movementController;
    private LookOrientation lookOrientation;
    private float scanDistance = 1f;
    private bool isBusy = false;
    private bool isSenterEnabled;
    private Vector3 playerDir;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += OnSenterPerformed;
        playerInputActions.Player.Enable();

        movementController = GetComponent<MovementController>();
        lookOrientation = GetComponent<LookOrientation>();
    }

    private void Update()
    {
        HandlePlayerAction();
    }

    private void HandlePlayerAction()
    {
        if (isBusy)
        {
            return;
        }

        Vector2 inputVector = playerInputActions.Player.MovePassThrough.ReadValue<Vector2>();
        
        if (Math.Abs(inputVector.x) == Math.Abs(inputVector.y)) // biar gabisa gerak diagonal
        {  
           return;
        }

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
        isSenterEnabled = !isSenterEnabled;
        senter.enabled = isSenterEnabled;
    }

    // biar posisi terakhir player nya ke save
    // public void OnApplicationQuit() {
    //     if (moveTarget != Vector3.zero) {
    //         temporarySaveDataSO.level01.playerSpawnPosition = moveTarget;
    //     } else {
    //         temporarySaveDataSO.level01.playerSpawnPosition = transform.position;
    //     }
    // }

}
