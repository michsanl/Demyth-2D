using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private MovementController movementController;
    [SerializeField] private LookOrientation lookOrientation;
    [SerializeField] private Animator animator;

    [SerializeField] private TemporarySaveDataSO temporarySaveDataSO;
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;

    private float scanDistance = 1f;
    private bool isBusy = false;
    private Vector3 playerDir;

    private void Start() 
    {
        transform.position = temporarySaveDataSO.level01.playerSpawnPosition; // load posisi terakhir
    }

    private void Update()
    {
        HandlePlayerAction();
    }

    private void HandlePlayerAction()
    {
        if (GameManager.Instance.State != GameState.Play || isBusy)
        {
            return;
        }

        Vector2 inputVector = gameInput.GetMovementVectorPassThrough(); 
        
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
            case InteractableType.Talkable:
                interactable.Interact();
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            case InteractableType.Pushable:
                interactable.Interact(playerDir);
                animator.SetTrigger("Attack");
                yield return Helper.GetWaitForSeconds(actionDelay);
                break;
            case InteractableType.LevelChanger:
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
