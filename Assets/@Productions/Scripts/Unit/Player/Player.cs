using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Demyth.Gameplay;
using echo17.Signaler.Core;
using Core;
using MoreMountains.Tools;

public class Player : MonoBehaviour, IBroadcaster
{
    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private AudioClipAraSO araAudioSO;
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

    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        lookOrientation = GetComponent<LookOrientation>();
        healthPotion = GetComponent<HealthPotion>();
        health = GetComponent<Health>();

        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _gameInput = _gameInputController.GameInput;
    }

    private void Start()
    {
        Signaler.Instance.Broadcast(this, new PlayerSpawnEvent { Player = gameObject });
    }

    private void Update()
    {
        HandlePlayerAction();
    }

    private void OnEnable()
    {
        moveTargetPosition = transform.position;

        _gameInput.OnSenterPerformed.AddListener(GameInput_OnSenterPerformed);
        _gameInput.OnHealthPotionPerformed.AddListener(GameInput_OnHealthPotionPerformed);
    }

    private void OnDisable()
    {
        _gameInput.OnSenterPerformed.RemoveListener(GameInput_OnSenterPerformed);
        _gameInput.OnHealthPotionPerformed.RemoveListener(GameInput_OnHealthPotionPerformed);
    }

    public void ApplyDamageToPlayer(bool enableKnockBack, Vector2 knockbackTargetPosition)
    {
        if (isTakeDamageOnCooldown)
            return;
        if (enableKnockBack)
            isKnocked = true;

        StartCoroutine(ApplyDamageToPlayerCoroutine(enableKnockBack, knockbackTargetPosition));
    }

    public void ApplyKnockBackToPlayer(Vector2 knockbackTargetPosition)
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

        Vector2 inputVector = _gameInput.GetMovementVector();

        if (inputVector == Vector2.zero)
            return;
        if (IsInputVectorDiagonal(inputVector))
            return;

        playerDir = inputVector;
        lookOrientation.SetFacingDirection(playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, playerDir, moveBlockMask, out Interactable interactable))
        {
            if (interactable == null)
                return;

            StartCoroutine(HandleInteract(interactable));
        }
        else
        {
            StartCoroutine(HandleMovement());
        }
    }

    private IEnumerator HandleMovement()
    {
        isBusy = true;

        animator.SetTrigger("Dash");
        PlayAudio(araAudioSO.Move);

        moveTargetPosition = GetMoveTargetPosition();
        Helper.MoveToPosition(transform, moveTargetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);

        isBusy = false;
    }

    private IEnumerator HandleInteract(Interactable interactable)
    {
        isBusy = true;

        switch (interactable)
        {
            case Pushable:
                animator.SetTrigger("Attack");
                interactable.Interact(this, playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case Damageable:
                animator.SetTrigger("Attack");
                interactable.Interact(this, playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                isBusy = false;
                yield break;
            case PillarLight:
                animator.SetTrigger("Attack");
                interactable.Interact(this, playerDir);
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

        PlayAudio(araAudioSO.Potion);
        healthPotion.UsePotion();
    }

    private IEnumerator ToggleSenter()
    {
        if (senterGameObject.activeInHierarchy)
        {
            // Move the object away to trigger OnCollisonExit
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

        PlayAudio(araAudioSO.Lantern);
        OnSenterToggle?.Invoke(isSenterEnabled);
    }

    private IEnumerator ApplyDamageToPlayerCoroutine(bool knockBackPlayer, Vector2 knockbackTargetPosition)
    {
        isTakeDamageOnCooldown = true;

        animator.SetTrigger("OnHit");

        //yield return StartCoroutine(Context.CameraShakeController.PlayCameraShake());

        health.TakeDamage();

        damagedAnimator.Play("Ara_Damaged");
        PlayAudio(araAudioSO.MoveBox[UnityEngine.Random.Range(0, 3)]);

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

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
    
    public void ActivatePlayer()
    {
        gameObject.SetActive(true);
        isBusy = false;
        isKnocked = false;
    }

    private Vector2 GetMoveTargetPosition()
    {
        var moveTargetPosition = (Vector2)transform.position + playerDir;
        moveTargetPosition.x = Mathf.RoundToInt(moveTargetPosition.x);
        moveTargetPosition.y = Mathf.RoundToInt(moveTargetPosition.y);
        return moveTargetPosition;
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
