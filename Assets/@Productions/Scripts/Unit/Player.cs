using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomTools.Core;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Player : SceneService
{
    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    [SerializeField] private GameObject hitEffect;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private GameObject senterGameObject;
    
#region Public Fields
    
    public Action OnInvulnerableVisualStart;
    public Action OnInvulnerableVisualEnd;
    public Action<bool> OnSenterToggle;
    public Vector2 LastMoveTargetPosition => moveTargetPosition;
    public Vector2 PlayerDir => playerDir; 

#endregion

    private CameraShakeController cameraShakeController;
    private FlashEffectController flashEffectController;
    private LookOrientation lookOrientation;
    private HealthPotion healthPotion;
    private Health health;
    private Vector2 playerDir;
    private Vector2 moveTargetPosition;
    private bool isBusy;
    private bool isKnocked;
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
    }

    protected override void OnActivate()
    {
        base.OnActivate();

        Context.GameInput.OnSenterPerformed += GameInput_OnSenterPerformed;
        Context.GameInput.OnHealthPotionPerformed += GameInput_OnHealthPotionPerformed;

        moveTargetPosition = transform.position;
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
        if (!isActiveAndEnabled)
            return;
        if (isKnocked)
            return;
        if (isBusy)
            return;

        Vector2 inputVector = Context.GameInput.GetMovementVector();

        if (inputVector == Vector2.zero)
            return;
        if (IsInputVectorDiagonal(inputVector))
            return;

        playerDir = inputVector;
        moveTargetPosition = GetMoveTargetPosition();
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, moveBlockMask, out Interactable interactable))
        {
            if (interactable != null)
            {
                StartCoroutine(HandleInteract(interactable, moveTargetPosition));
            }
        } 
        else
        {
            StartCoroutine(HandleMovement());
        }
    }

    private Vector2 GetMoveTargetPosition()
    {
        var moveTargetPosition = (Vector2)transform.position + playerDir;
        moveTargetPosition.x = Mathf.RoundToInt(moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt(moveTargetPosition.y);
        return moveTargetPosition;
    }

    private IEnumerator HandleMovement()
    {
        isBusy = true;

        animator.SetTrigger("Dash");
        Helper.MoveToPosition(transform, moveTargetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);

        isBusy = false;
    }

    private IEnumerator HandleInteract(Interactable interactable, Vector2 moveTargetPosition)
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
                Instantiate(hitEffect, moveTargetPosition, Quaternion.identity);
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case InteractableType.PillarLight:
                animator.SetTrigger("Attack");
                interactable.Interact();
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case InteractableType.HiddenItem:
                interactable.Interact();
                yield return StartCoroutine(HandleMovement());
                yield break;
            default:
                break;
        }
        interactable.Interact(playerDir);

        yield return Helper.GetWaitForSeconds(actionDuration);

        isBusy = false;
    }

    private void GameInput_OnSenterPerformed()
    {
        if (Time.deltaTime == 0)
            return;
        if (!isActiveAndEnabled)
            return;
        if (!isSenterUnlocked)
            return;
        StartCoroutine(ToggleSenter());
    }

    private void GameInput_OnHealthPotionPerformed()
    {
        if (Time.deltaTime == 0)
            return;
        if (!isActiveAndEnabled)
            return;
        if (!isHealthPotionUnlocked)
            return;
        healthPotion.UsePotion();
    }

    private IEnumerator ToggleSenter()
    {
        if (senterGameObject.activeInHierarchy)
        {
            senterGameObject.transform.localPosition = new Vector3(100, 100, 0);
            // yield return senterGameObject.transform.DOMoveX(100f, 0f).WaitForCompletion();
            yield return Helper.GetWaitForSeconds(0.05f);
            senterGameObject.SetActive(false);
            isSenterEnabled = false;
        }
        else
        {
            senterGameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            yield return Helper.GetWaitForSeconds(0.05f);
            senterGameObject.SetActive(true);
            isSenterEnabled = true;
        }

        OnSenterToggle?.Invoke(isSenterEnabled);
    }

    public void TakeDamage(bool enableKnockBack, Vector2 knockbackTargetPosition)
    {
        if (isTakeDamageOnCooldown)
            return;
        if (enableKnockBack)
            isKnocked = true;

        StartCoroutine(TakeDamageRoutine(enableKnockBack, knockbackTargetPosition));
    }

    private IEnumerator TakeDamageRoutine(bool knockBackPlayer, Vector2 knockbackTargetPosition)
    {
        isTakeDamageOnCooldown = true;

        animator.SetTrigger("OnHit");

        yield return StartCoroutine(cameraShakeController.PlayCameraShake());

        health.TakeDamage();

        damagedAnimator.Play("Ara_Damaged");

        if (knockBackPlayer)
            StartCoroutine(HandleKnockBack(knockbackTargetPosition));
        
        OnInvulnerableVisualStart?.Invoke();
        yield return Helper.GetWaitForSeconds(takeDamageCooldown);
        OnInvulnerableVisualEnd?.Invoke();

        isTakeDamageOnCooldown = false;
    }

    private IEnumerator HandleKnockBack(Vector2 targetPosition)
    {
        isKnocked = true;

        moveTargetPosition = targetPosition;
        Helper.MoveToPosition(transform, targetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);

        isKnocked = false;
    }

    public void ActivatePlayer()
    {
        gameObject.SetActive(true);
        isBusy = false;
        isKnocked = false;
    }

    private Vector2 GetOppositeDirection(Vector2 dir)
    {
        dir.x = Mathf.RoundToInt(dir.x * -1f); 
        dir.y = Mathf.RoundToInt(dir.y * -1f);
        return dir;
    }
    
    private bool IsInputVectorDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }
}
