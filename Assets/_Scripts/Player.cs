using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    public event EventHandler OnMove;
    public event EventHandler<OnMovementInputPressedEventArgs> OnMovementInputPressed;
    public class OnMovementInputPressedEventArgs
    {
        public float inputVectorX;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private TemporarySaveDataSO temporarySaveDataSO;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private LayerMask osbtacleLayerMask;
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;

    private float scanDistance = 1f;
    private bool isBusy;
    private Vector3 playerDir;
    private Vector3 moveTarget;

    private void Start() 
    {
        isBusy = false;
        transform.position = temporarySaveDataSO.level01.playerSpawnPosition;
    }

    private void Update()
    {
        if (GameManager.Instance.State == FirstLevelState.Play)
        {
            if (!isBusy)
            {
                HandlePlayerAction();
            }
        }
    }

    private void HandlePlayerAction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorPassThrough();
        
        if (Math.Abs(inputVector.x)  == Math.Abs(inputVector.y))
        {  
           return; 
        }

        if (inputVector.x != 0)
        {
            OnMovementInputPressed?.Invoke(this, new OnMovementInputPressedEventArgs
            {
                inputVectorX = inputVector.x
            });
        }

        playerDir = new Vector3(inputVector.x, inputVector.y, 0);

        if (!Physics2D.Raycast(transform.position, playerDir, scanDistance, osbtacleLayerMask))
        {
            StartCoroutine(Move());
        } else 
        {
            StartCoroutine(TryInteract());
            StartCoroutine(TryPush());
        }
    }

    private IEnumerator Move() {
        isBusy = true;
        moveTarget = transform.position + playerDir;
        OnMove?.Invoke(this,EventArgs.Empty);
        transform.DOMove(moveTarget, moveDuration).SetEase(Ease.OutExpo);
        yield return Helper.GetWait(actionDelay);
        isBusy = false;
    }

    private IEnumerator TryInteract() {
        isBusy = true;
        RaycastHit2D raycasthit = Physics2D.Raycast(transform.position, playerDir, scanDistance, interactLayerMask);
        if (raycasthit != false)
        {
            if (raycasthit.transform.TryGetComponent (out Interactable interactable)) {
                interactable.Interact();
                yield return Helper.GetWait(actionDelay);
            }
        }
        isBusy = false;
    }

    private IEnumerator TryPush() 
    {
        isBusy = true;
        RaycastHit2D raycasthit = Physics2D.Raycast(transform.position, playerDir, scanDistance, interactLayerMask);
        if (raycasthit != false)
        {
            if (raycasthit.transform.TryGetComponent (out Interactable interactable)) {
                interactable.Push(playerDir, moveDuration);
                yield return Helper.GetWait(actionDelay);
            }
        }
        isBusy = false;
    }

    public void OnApplicationQuit() {
        if (moveTarget != Vector3.zero) {
            temporarySaveDataSO.level01.playerSpawnPosition = moveTarget;
        } else {
            temporarySaveDataSO.level01.playerSpawnPosition = transform.position;
        }
    }
    

    
}
