using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomTools.Core;
using UnityEngine.InputSystem;
using UISystem;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;

public class Player : SceneService
{
    [Title("Settings")]
    [SerializeField] private float actionDelay;
    [SerializeField] private float moveDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float invulnerableDuration = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject senterGameObject;
    
#region Public Fields
    
    public Action OnInvulnerableVisualStart;
    public Action OnInvulnerableVisualEnd;
    public Action<bool> OnSenterToggle;
    public Vector2 LastMoveTargetPosition => moveTargetPosition; 

#endregion

    private CameraShakeController cameraShakeController;
    private FlashEffectController flashEffectController;
    private LookOrientation lookOrientation;
    private HealthPotion healthPotion;
    private MeshRenderer spineMeshRenderer;
    private Health health;
    private Vector2 playerDir;
    private Vector2 moveTargetPosition;
    private Vector3 lastPositionBeforeMove;
    private bool isBusy;
    private bool isBeingHit;
    private bool isKnocked;
    private bool isInvulnerable;
    private bool isSenterEnabled;
    private bool isSenterUnlocked = true;
    private bool isHealthPotionUnlocked = true;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        cameraShakeController = GetComponent<CameraShakeController>();
        flashEffectController = GetComponent<FlashEffectController>();
        lookOrientation = GetComponent<LookOrientation>();
        healthPotion = GetComponent<HealthPotion>();
        health = GetComponent<Health>();
        spineMeshRenderer = animator.GetComponent<MeshRenderer>();
    }

    protected override void OnActivate()
    {
        base.OnActivate();

        Context.gameInput.OnSenterPerformed += GameInput_OnSenterPerformed;
        Context.gameInput.OnHealthPotionPerformed += GameInput_OnHealthPotionPerformed;
    }

    protected override void OnTick()
    {
        base.OnTick();

        HandlePlayerAction();
    }

    private void HandlePlayerAction()
    {
        if (Time.deltaTime == 0)
            return;
        if (isKnocked)
            return;
        if (isBusy)
            return;

        playerDir = Context.gameInput.GetMovementVector();

        if (playerDir == Vector2.zero)
            return;
        if (IsDirectionDiagonal(playerDir))
            return;
        
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, moveBlockMask, out Interactable interactable))
        {
            if (interactable != null)
            {
                StartCoroutine(HandleInteract(interactable));
            }
        } 
        else
        {
            SetMoveTargetPosition();
            StartCoroutine(HandleMovement());
        }
    }

    private void SetMoveTargetPosition()
    {
        moveTargetPosition = (Vector2)transform.position + playerDir;
        moveTargetPosition.x = Mathf.RoundToInt(moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt(moveTargetPosition.y);
    }

    private IEnumerator HandleMovement()
    {
        isBusy = true;

        Helper.MoveToPosition(transform, moveTargetPosition, moveDuration);
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
                interactable.Interact();
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            default:
                break;
        }

        interactable.Interact(playerDir);

        yield return Helper.GetWaitForSeconds(actionDelay);

        isBusy = false;
    }

    private void GameInput_OnSenterPerformed()
    {
        if (Time.deltaTime == 0)
            return;
        if (!isSenterUnlocked)
            return;
        ToggleSenter();
    }

    private void GameInput_OnHealthPotionPerformed()
    {
        if (Time.deltaTime == 0)
            return;
        if (!isHealthPotionUnlocked)
            return;
        healthPotion.UsePotion();
    }

    private void ToggleSenter()
    {
        isSenterEnabled = !isSenterEnabled;
        senterGameObject.SetActive(isSenterEnabled);

        OnSenterToggle?.Invoke(isSenterEnabled);
    }

    public void TakeDamage()
    {
        if (isInvulnerable)
            return;

        StartCoroutine(TakeDamageRoutine());
    }

    private IEnumerator TakeDamageRoutine()
    {
        isInvulnerable = true;

        animator.SetTrigger("OnHit");
        health.TakeDamage(1);

        yield return StartCoroutine(cameraShakeController.PlayCameraShake());
        
        OnInvulnerableVisualStart?.Invoke();
        yield return Helper.GetWaitForSeconds(invulnerableDuration);
        OnInvulnerableVisualEnd?.Invoke();

        isInvulnerable = false;
    }

    public void KnockBack(Vector2 dir)
    {
        if (isKnocked)
            return;

        StartCoroutine(HandleKnockBack(dir));
    }

    private IEnumerator HandleKnockBack(Vector2 dir)
    {
        isKnocked = true;

        if (!Helper.CheckTargetDirection(moveTargetPosition, dir, moveBlockMask, out Interactable interactable))
        {
            moveTargetPosition = moveTargetPosition + dir;
            Helper.MoveToPosition(transform, moveTargetPosition, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        }

        isKnocked = false;
    }
    
    private bool IsDirectionDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }
}
