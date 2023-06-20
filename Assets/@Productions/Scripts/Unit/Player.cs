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

public class Player : CoreBehaviour
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
    
    public Vector2 LastMoveTargetPosition => lastMoveTargetPosition; 
    public bool IsGamePaused => isGamePaused;

#endregion

    private PlayerInputActions playerInputActions;
    private CameraShakeController cameraShakeController;
    private FlashEffectController flashEffectController;
    private LookOrientation lookOrientation;
    private MeshRenderer spineMeshRenderer;
    private Health health;
    private Vector2 playerDir;
    private Vector2 lastMoveTargetPosition;
    private bool isBusy;
    private bool isKnocked;
    private bool isTakeDamageOnCooldown;
    private bool isSenterEnabled;
    private bool isSenterUnlocked = true;
    private bool isHealthPotionOnCooldown;
    private bool isHealthPotionUnlocked = true;
    private bool isGamePaused;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += OnHealthPotionPerformed;
        playerInputActions.Player.Pause.performed += OnPausePerformed;

        playerInputActions.Player.Enable();

        cameraShakeController = GetComponent<CameraShakeController>();
        flashEffectController = GetComponent<FlashEffectController>();
        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
        spineMeshRenderer = animator.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
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
    
    private void OnSenterPerformed(InputAction.CallbackContext context)
    {
        if (!isSenterUnlocked)
            return;

        ToggleSenter();

        Context.HUDUI.SetActiveSenterImage(isSenterEnabled);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        ToggleGamePause();

        Context.UI.Toggle<PauseUI>();
    }

    private void OnHealthPotionPerformed(InputAction.CallbackContext context)
    {
        if (!isHealthPotionUnlocked)
            return;
        if (isHealthPotionOnCooldown)
            return;
        
        StartCoroutine(HealSelf());
    }

    private void ToggleSenter()
    {
        isSenterEnabled = !isSenterEnabled;
        senterGameObject.SetActive(isSenterEnabled);
    }

    public void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            Context.VCamCameraShake.gameObject.SetActive(false);
        } else
        {
            Time.timeScale = 1f;
        }
    }

    private IEnumerator HealSelf()
    {
        isHealthPotionOnCooldown = true;

        health.Heal(1);

        yield return Helper.GetWaitForSeconds(actionDelay);

        isHealthPotionOnCooldown = false;
    }

    public IEnumerator DamagePlayer(bool enableCameraShake, bool enableKnockback, Vector2 knockBackDir)
    {
        if (isTakeDamageOnCooldown)
            yield break;
        
        animator.SetTrigger("OnHit");
        health.TakeDamage(1);

        if (enableCameraShake)
            yield return StartCoroutine(cameraShakeController.PlayCameraShake());

        StartCoroutine(HandleFlashEffectOnHit());

        if (enableKnockback)
            yield return StartCoroutine(HandleKnockBack(knockBackDir));
    }

    private IEnumerator HandleFlashEffectOnHit()
    {
        isTakeDamageOnCooldown = true;

        yield return StartCoroutine(flashEffectController.PlayFlashEffect());

        isTakeDamageOnCooldown = false;
    }

    private IEnumerator HandleKnockBack(Vector2 dir)
    {
        isKnocked = true;

        if (!Helper.CheckTargetDirection(lastMoveTargetPosition, dir, movementBlockerLayerMask, out Interactable interactable))
        {
            lastMoveTargetPosition = lastMoveTargetPosition + dir;
            Helper.MoveToPosition(transform, lastMoveTargetPosition, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        }
        
        isKnocked = false;
    }
    
    private bool IsDirectionDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }
}
