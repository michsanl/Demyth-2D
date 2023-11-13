using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Demyth.Gameplay;
using echo17.Signaler.Core;

public class Player : MonoBehaviour, IBroadcaster
{
    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private bool disableOnStart;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private GameObject senterGameObject;
    [SerializeField] private GameObject hitEffect;
    
#region Public Fields
    
    public Action OnInvulnerableVisualStart;
    public Action OnInvulnerableVisualEnd;
    public Action<bool> OnSenterToggle;
    public Vector2 LastMoveTargetPosition => moveTargetPosition;
    public Vector2 PlayerDir => playerDir;

#endregion

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

    private void Awake()
    {
        lookOrientation = GetComponent<LookOrientation>();
        healthPotion = GetComponent<HealthPotion>();
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        //TODO :: REFACTOR THESE
        Damageable.OnAnyDamageableInteract += Damageable_OnAnyDamageableInteract;
        Pickupable.OnAnyPickupableInteract += Pickupable_OnAnyPickupableInteract;
        PillarLight.OnAnyPillarLightInteract += PillarLight_OnAnyPillarLightInteract;
        Pushable.OnAnyPushableInteract += Pushable_OnAnyPushableInteract;
        Talkable.OnAnyTalkbleInteract += Talkable_OnAnyTalkbleInteract;
        Gate.OnAnyGateInteract += Gate_OnAnyGateInteract;

        moveTargetPosition = transform.position;

        if (disableOnStart)
            gameObject.SetActive(false);

        Signaler.Instance.Broadcast(this, new PlayerSpawnEvent { Player = gameObject });
    }

    private void OnDisable()
    {
        Damageable.OnAnyDamageableInteract -= Damageable_OnAnyDamageableInteract;
        Pickupable.OnAnyPickupableInteract -= Pickupable_OnAnyPickupableInteract;
        PillarLight.OnAnyPillarLightInteract -= PillarLight_OnAnyPillarLightInteract;
        Pushable.OnAnyPushableInteract -= Pushable_OnAnyPushableInteract;
        Talkable.OnAnyTalkbleInteract -= Talkable_OnAnyTalkbleInteract;
        Gate.OnAnyGateInteract -= Gate_OnAnyGateInteract;
    }

    private void Update()
    {
        HandlePlayerAction();
    }
    
    public void ActivatePlayer()
    {
        gameObject.SetActive(true);
        isBusy = false;
        isKnocked = false;
    }

    public void TriggerKnockBack(Vector2 knockbackTargetPosition)
    {
        animator.SetTrigger("OnHit");
        StartCoroutine(HandleKnockBack(knockbackTargetPosition));
    }
    private void HandlePlayerAction()
    {
        if (isKnocked)
            return;
        if (isBusy)
            return;

        //TODO:: READ MOVEMENT INPUT VECTOR
        Vector2 inputVector = Vector3.zero;

        if (inputVector == Vector2.zero)
            return;
        if (IsInputVectorDiagonal(inputVector))
            return;

        playerDir = inputVector;
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, moveBlockMask, out Interactable interactable))
        {
            interactable?.Interact(this, playerDir);
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

        moveTargetPosition = GetMoveTargetPosition();
        Helper.MoveToPosition(transform, moveTargetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);

        isBusy = false;
    }

    private void GameInput_OnSenterPerformed()
    {
        if (!isSenterUnlocked)
            return;
        StartCoroutine(ToggleSenter());
    }

    private void GameInput_OnHealthPotionPerformed()
    {
        if (!isHealthPotionUnlocked)
            return;
        if (health.IsHealthFull())
            return;
        if (healthPotion.CurrentPotionAmount <= 0) 
            return;
        if (healthPotion.IsHealthPotionOnCooldown)
            return;

        healthPotion.UsePotion();
    }

    private IEnumerator ToggleSenter()
    {
        if (senterGameObject.activeInHierarchy)
        {
            senterGameObject.transform.localPosition = new Vector3(100, 100, 0);
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

    public void TakeDamage(bool enableKnockBack, Vector2 knockbackTargetPosition, PlayerDamager.DamagerCharacter damager)
    {
        if (isTakeDamageOnCooldown)
            return;
        if (enableKnockBack)
            isKnocked = true;

        StartCoroutine(TakeDamageRoutine(enableKnockBack, knockbackTargetPosition, damager));
    }

    private IEnumerator TakeDamageRoutine(bool knockBackPlayer, Vector2 knockbackTargetPosition, PlayerDamager.DamagerCharacter damager)
    {
        isTakeDamageOnCooldown = true;

        animator.SetTrigger("OnHit");

        //yield return StartCoroutine(Context.CameraShakeController.PlayCameraShake());

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

    // old interact method
    private IEnumerator HandleInteract(Interactable interactable)
    {
        isBusy = true;

        switch (interactable)
        {
            case Pushable:
                animator.SetTrigger("Attack");                
                Instantiate(hitEffect, GetMoveTargetPosition(), Quaternion.identity);
                break;
            case Damageable:
                animator.SetTrigger("Attack");
                Instantiate(hitEffect, GetMoveTargetPosition(), Quaternion.identity);
                interactable.Interact(this);
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case PillarLight:
                animator.SetTrigger("Attack");
                interactable.Interact(this);
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case Pickupable:
                interactable.Interact(this);
                yield return StartCoroutine(HandleMovement());
                isBusy = false;
                yield break;
            default:
                break;
        }

        interactable.Interact(this, playerDir);
        yield return Helper.GetWaitForSeconds(actionDuration);

        isBusy = false;
    }

#region New Interact Logic

    private void Damageable_OnAnyDamageableInteract()
    {
        StartCoroutine(DamageableCallback());
    }

    private void Pickupable_OnAnyPickupableInteract()
    {
        StartCoroutine(PickupableCallback());
    }

    private void PillarLight_OnAnyPillarLightInteract()
    {
        StartCoroutine(PillarLightCallback());
    }

    private void Pushable_OnAnyPushableInteract()
    {
        StartCoroutine(PushableCallback());
    }

    private void Talkable_OnAnyTalkbleInteract()
    {
        StartCoroutine(DefaultCallback());
    }

    private void Gate_OnAnyGateInteract()
    {
        StartCoroutine(DefaultCallback());
    }

    public IEnumerator PushableCallback()
    {
        isBusy = true;
        animator.SetTrigger("Attack");
        Instantiate(hitEffect, GetMoveTargetPosition(), Quaternion.identity);
        yield return Helper.GetWaitForSeconds(attackDuration);
        isBusy = false;
    }

    public IEnumerator DamageableCallback()
    {
        isBusy = true;
        animator.SetTrigger("Attack");
        Instantiate(hitEffect, GetMoveTargetPosition(), Quaternion.identity);
        yield return Helper.GetWaitForSeconds(attackDuration);
        isBusy = false;
    }

    public IEnumerator PillarLightCallback()
    {
        isBusy = true;
        animator.SetTrigger("Attack");
        Instantiate(hitEffect, GetMoveTargetPosition(), Quaternion.identity);
        yield return Helper.GetWaitForSeconds(attackDuration);
        isBusy = false;
    }

    public IEnumerator PickupableCallback()
    {
        isBusy = true;
        yield return StartCoroutine(HandleMovement());
        isBusy = false;
    }

    public IEnumerator DefaultCallback()
    {
        isBusy = true;
        yield return Helper.GetWaitForSeconds(actionDuration);
        isBusy = false;
    }

#endregion     
}
