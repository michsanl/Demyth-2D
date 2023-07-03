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
    [SerializeField] private LayerMask movementBlockerLayerMask;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject senterGameObject;
    
#region Public Fields
    
    public Action OnMove;
    public Action<bool> OnSenterToggle;
    public Vector2 LastMoveTargetPosition => lastMoveTargetPosition; 

#endregion

    private CameraShakeController cameraShakeController;
    private FlashEffectController flashEffectController;
    private LookOrientation lookOrientation;
    private HealthPotion healthPotion;
    private MeshRenderer spineMeshRenderer;
    private Health health;
    private Vector2 playerDir;
    private Vector2 lastMoveTargetPosition;
    private bool isBusy;
    private bool isBeingHit;
    private bool isTakeDamageOnCooldown;
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
        if (isBeingHit)
            return;
        if (isBusy)
            return;

        playerDir = Context.gameInput.GetMovementVector();

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
        lastMoveTargetPosition = (Vector2)transform.position + playerDir;
        lastMoveTargetPosition.x = Mathf.RoundToInt(lastMoveTargetPosition.x);
        lastMoveTargetPosition.y = Mathf.RoundToInt(lastMoveTargetPosition.y);
    }

    private IEnumerator HandleMovement()
    {
        isBusy = true;

        Helper.MoveToPosition(transform, lastMoveTargetPosition, moveDuration);
        animator.SetTrigger("Dash");
        OnMove?.Invoke();
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

    public IEnumerator DamagePlayer(bool enableCameraShake, bool enableKnockback, Vector2 knockBackDir)
    {
        if (isTakeDamageOnCooldown)
            yield break;

        isTakeDamageOnCooldown = true;

        StartCoroutine(TakingDamage());

        if (enableCameraShake)
            yield return StartCoroutine(cameraShakeController.PlayCameraShake());

        StartCoroutine(HandleFlashEffectOnHit());

        if (enableKnockback)
            StartCoroutine(HandleKnockBack(knockBackDir));
    }

    private IEnumerator TakingDamage()
    {
        isBeingHit = true;
        
        animator.SetTrigger("OnHit");
        health.TakeDamage(1);
        yield return Helper.GetWaitForSeconds(actionDelay);

        isBeingHit = false;
    }

    private IEnumerator HandleFlashEffectOnHit()
    {
        isTakeDamageOnCooldown = true;

        yield return StartCoroutine(flashEffectController.PlayFlashEffect());
        
        isTakeDamageOnCooldown = false;
    }

    private IEnumerator HandleKnockBack(Vector2 dir)
    {
        if (!Helper.CheckTargetDirection(lastMoveTargetPosition, dir, movementBlockerLayerMask, out Interactable interactable))
        {
            lastMoveTargetPosition = lastMoveTargetPosition + dir;
            Helper.MoveToPosition(transform, lastMoveTargetPosition, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        }
    }
    
    private bool IsDirectionDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }
}
