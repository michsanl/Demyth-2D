using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;

    private PlayerInputActions playerInputActions;
    private MovementController movementController;
    private LookOrientation lookOrientation;
    private float scanDistance = 1f;
    private bool isBusy = false;
    private Vector3 playerDir;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
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

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, playerDir, scanDistance, movementBlockerLayerMask);
        if (raycastHit)
        {
            if (raycastHit.transform.TryGetComponent(out Interactable interactable))
            {
                StartCoroutine(HandleInteract(interactable));
            }
        } else
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
            case InteractableType.Talk:
                interactable.Interact();
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            case InteractableType.Push:
                interactable.Interact(playerDir);
                animator.SetTrigger("Attack");
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            case InteractableType.Damage:
                interactable.Interact(playerDir);
                animator.SetTrigger("Attack");
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            case InteractableType.ChangeLevel:
                interactable.Interact();
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            default:
                break;
        }

        isBusy = false;
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
