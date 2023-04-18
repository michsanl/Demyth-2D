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
    private bool isBusy = false;
    private Vector3 playerDir;
    private Vector3 moveTarget;

    private void Start() 
    {
        transform.position = temporarySaveDataSO.level01.playerSpawnPosition; // load posisi terakhir
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Play) // kalo game state nya lagi Mainmenu atau pause etc, ga bisa action
        {
            if (!isBusy) // ngatur action delay, biar move/interact ga ke spam tiap frame
            {
                HandlePlayerAction();
            }
        }
    }

    // 
    private void HandlePlayerAction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorPassThrough(); // get InputAction WASD value vector2 
        
        if (Math.Abs(inputVector.x)  == Math.Abs(inputVector.y)) // logic sementaraga biar ga bisa gerak diagonal
        {  
           return; 
        }

        if (inputVector.x != 0)  // ngatur madep kanan kiri
        {
            OnMovementInputPressed?.Invoke(this, new OnMovementInputPressedEventArgs
            {
                inputVectorX = inputVector.x
            });
        }

        playerDir = inputVector;

        // raycast ke depan, target layer mask nya = wall, NPC, box
        if (!Physics2D.Raycast(transform.position, playerDir, scanDistance, osbtacleLayerMask))
        {
            // kalo player ga nabrak wall NPC atau box, boleh move
            StartCoroutine(Move());
        } else 
        {
            // kalo nabrak wall NPC sama box, raycast ke Interactable(box & NPC) lalu interact
            StartCoroutine(TryInteract());
        }
    }

    // grid movement
    private IEnumerator Move() 
    {
        isBusy = true;

        OnMove?.Invoke(this,EventArgs.Empty); // trigger animasi dash

        moveTarget = transform.position + playerDir;
        transform.DOMove(moveTarget, moveDuration).SetEase(Ease.OutExpo);

        yield return Helper.GetWait(actionDelay);  // non allocating WaitForSeconds semoga jadi ga bloodware, buat action delay
        isBusy = false;
    }

    // raycast ke arah depan, layer target nya = box & NPC
    // kalo kena, ngecall fungsi Talk sama Push dari class Interactable
    private IEnumerator TryInteract() {
        isBusy = true;
        RaycastHit2D raycasthit = Physics2D.Raycast(transform.position, playerDir, scanDistance, interactLayerMask);
        if (raycasthit != false)
        {
            if (raycasthit.transform.TryGetComponent(out Interactable interactable)) 
            {
                interactable.Talk();
                interactable.Push(playerDir, moveDuration);
                yield return Helper.GetWait(actionDelay); // non allocating WaitForSeconds semoga jadi ga bloodware, buat action delay
            }
        }
        isBusy = false;
    }

    // biar posisi terakhir player nya ke save
    public void OnApplicationQuit() {
        if (moveTarget != Vector3.zero) {
            temporarySaveDataSO.level01.playerSpawnPosition = moveTarget;
        } else {
            temporarySaveDataSO.level01.playerSpawnPosition = transform.position;
        }
    }
    

    
}
