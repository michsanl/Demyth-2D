using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    public event EventHandler OnMove;
    public event EventHandler OnPush;
    public event EventHandler<OnMovementInputPressedEventArgs> OnMovementInputPressed;
    public class OnMovementInputPressedEventArgs
    {
        public float inputVectorX;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private MovementController movementController;

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

    private void FixedUpdate()
    {
        StartCoroutine(HandlePlayerAction());
    }

    private IEnumerator HandlePlayerAction()
    {
        if (GameManager.Instance.State != GameState.Play || isBusy)
        {
            yield break;
        }

        Vector2 inputVector = gameInput.GetMovementVectorPassThrough(); // get InputAction WASD value vector2 
        
        if (Math.Abs(inputVector.x)  == Math.Abs(inputVector.y))
        {  
           yield break; 
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
            isBusy = true;

            movementController.Move(playerDir, moveDuration);
            OnMove?.Invoke(this,EventArgs.Empty); // trigger animasi dash
            yield return Helper.GetWait(actionDelay);

            isBusy = false;
        } else 
        {
            // kalo nabrak wall NPC sama box, raycast ke Interactable(box & NPC) lalu interact
            StartCoroutine(TryInteract());
        }
    }

    // raycast ke arah depan, layer target nya = box & NPC
    // kalo kena, ngecall fungsi Talk sama Push dari class Interactable
    private IEnumerator TryInteract() {
        isBusy = true;
        RaycastHit2D raycasthit = Physics2D.Raycast(transform.position, playerDir, scanDistance, interactLayerMask);
        if (raycasthit != false)
        {
            if (raycasthit.transform.TryGetComponent(out Pushable pushable)) 
            {
                pushable.Push(playerDir, moveDuration);

                OnPush?.Invoke(this,EventArgs.Empty); // trigger animasi push

                yield return Helper.GetWait(actionDelay); // non allocating WaitForSeconds semoga jadi ga bloodware, buat action delay
            }

            if (raycasthit.transform.TryGetComponent(out Talkable talkable)) 
            {
                talkable.Talk();
                yield return Helper.GetWait(actionDelay); // non allocating WaitForSeconds semoga jadi ga bloodware, buat action delay
            }

            if (raycasthit.transform.TryGetComponent(out LevelChanger levelChanger)) 
            {
                levelChanger.ChangeLevel();
                yield return Helper.GetWait(actionDelay); // non allocating WaitForSeconds semoga jadi ga bloodware, buat action delay
            }
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
