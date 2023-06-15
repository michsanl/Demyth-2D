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
    [SerializeField] private float screenShakeDuration;
    [SerializeField] private float invulnerableDuration;
    [SerializeField] private LayerMask movementBlockerLayerMask;
    
    [Title("External Component")]
    [SerializeField] private GameObject senterGameObject;
    [SerializeField] private Animator animator;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material flashingMaterial;
    

    public Vector2 MoveTargetPosition => moveTargetPosition; 

    private PlayerInputActions playerInputActions;
    private MovementController movementController;
    private LookOrientation lookOrientation;
    private MeshRenderer spineMeshRenderer;
    private SkeletonAnimation skeletonAnimation;
    private Health health;
    private Vector2 playerDir;
    private Vector2 moveTargetPosition;
    private bool isBusy;
    private bool isStunned;
    private bool isTakeDamageCooldown;
    private bool isSenterEnabled;
    private bool isSenterUnlocked = true;
    private bool isHealthPotionOnCooldown;
    private bool isHealthPotionUnlocked = true;

    public bool IsGamePaused => isGamePaused;
    private bool isGamePaused;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Senter.performed += OnSenterPerformed;
        playerInputActions.Player.HealthPotion.performed += OnHealthPotionPerformed;
        playerInputActions.Player.Pause.performed += OnPausePerformed;

        playerInputActions.Player.Enable();

        movementController = GetComponent<MovementController>();
        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
        spineMeshRenderer = animator.GetComponent<MeshRenderer>();
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();
    }
    
    private void Update()
    {
        HandlePlayerAction();
    }

    private void HandlePlayerAction()
    {
        if (isGamePaused)
            return;
        if (isStunned)
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
            moveTargetPosition = (Vector2)transform.position + playerDir;
            StartCoroutine(HandleMovement());
        }
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

    private IEnumerator PlayCameraShake()
    {
        GameObject cameraShakeGO = Context.VCamCameraShake.gameObject;
        
        Time.timeScale = 0;
        cameraShakeGO.SetActive(true);

        yield return new  WaitForSecondsRealtime(screenShakeDuration);

        if (!isGamePaused)
        {
            Time.timeScale = 1;
        }
        cameraShakeGO.SetActive(false);
    }

    private IEnumerator HandleKnockBack(Vector2 dir)
    {
        if (!Helper.CheckTargetDirection(moveTargetPosition, dir, movementBlockerLayerMask, out Interactable interactable))
        {
            moveTargetPosition = moveTargetPosition + dir;
            Helper.MoveToPosition(transform, moveTargetPosition, moveDuration);
            yield return Helper.GetWaitForSeconds(actionDelay);
        }

        isStunned = false;
    }

    public IEnumerator DamagePlayer(bool enableCameraShake, bool enableKnockback, Vector2 knockBackDir)
    {
        if (isTakeDamageCooldown)
            yield break;

        StartCoroutine(StartTakeDamageCooldown());

        isStunned = true;
        
        health.TakeDamage(1);

        if (enableCameraShake)
        {
            yield return StartCoroutine(PlayCameraShake());
        }

        StartCoroutine(FlashEffectOnTakeDamage());

        if (enableKnockback)
        {
            yield return StartCoroutine(HandleKnockBack(knockBackDir));
        }

        isStunned = false;
    }

    private IEnumerator FlashEffectOnTakeDamage()
    {
        MaterialPropertyBlock mpbNew = new MaterialPropertyBlock();
        mpbNew.SetColor("_RendererColor", Color.gray * .5f);
        // mpbNew.SetColor("_RendererColor", Color.red);

        MaterialPropertyBlock mpbVanilla = new MaterialPropertyBlock();
        mpbVanilla.Clear();

        for (int i = 0; i < 5; i++)
        {
            yield return Helper.GetWaitForSeconds(.15f);
            spineMeshRenderer.SetPropertyBlock(mpbNew);

            yield return Helper.GetWaitForSeconds(.1f);
            spineMeshRenderer.SetPropertyBlock(mpbVanilla);
        }
    }

    private IEnumerator FlashEffectMaterialSwap()
    {

        if (originalMaterial == null)
            originalMaterial = skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial;

        skeletonAnimation.CustomMaterialOverride[originalMaterial] = flashingMaterial; 

        yield return Helper.GetWaitForSeconds(1f);

        skeletonAnimation.CustomMaterialOverride.Remove(originalMaterial);
    }

    private IEnumerator StartTakeDamageCooldown()
    {
        isTakeDamageCooldown = true;

        yield return Helper.GetWaitForSeconds(invulnerableDuration);

        isTakeDamageCooldown = false;
    }
    
    private bool IsDirectionDiagonal(Vector2 direction)
    {
        return direction.x != 0 && direction.y != 0;
    }
    
    public Vector2 GetOppositeVector2Dir(Vector2 vector)
    {
        return new Vector2(vector.x * -1, vector.y * -1);
    }

}
