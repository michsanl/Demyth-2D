using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Demyth.Gameplay;
using echo17.Signaler.Core;
using Core;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class Player : MonoBehaviour, IBroadcaster
{
    public Vector2 LastMoveTargetPosition => _moveTargetPosition;
    public Vector2 PlayerDir => _playerDir;
    public bool IsDead => _isDead;
    public bool UsePan 
    {
        get => _usePan;
        set
        {
            _usePan = value;
            animator.SetBool("UsePan", value);
        }
    }

    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private AudioClipAraSO araAudioSO;

    private GameStateService _gameStateService;
    private LookOrientation _lookOrientation;
    private HealthPotion _healthPotion;
    private Health _health;
    private Shield _shield;
    private Lantern _lantern;
    private FlashEffectController _flashEffectController;
    private Vector2 _playerDir;
    private Vector2 _moveTargetPosition;
    private bool _isDead;
    private bool _isBusy;
    private bool _isKnocked;
    private bool _isTakeDamageOnCooldown;
    private bool _usePan;
    private bool _isLanternUnlocked = true;
    private bool _isHealthPotionUnlocked = true;

    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _lookOrientation = GetComponent<LookOrientation>();
        _healthPotion = GetComponent<HealthPotion>();
        _health = GetComponent<Health>();
        _shield = GetComponent<Shield>();
        _lantern = GetComponent<Lantern>();
        _flashEffectController = GetComponent<FlashEffectController>();

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
        _moveTargetPosition = transform.position;

        _gameInput.OnSenterPerformed.AddListener(GameInput_OnSenterPerformed);
        _gameInput.OnHealthPotionPerformed.AddListener(GameInput_OnHealthPotionPerformed);
    }

    private void OnDisable()
    {
        _gameInput.OnSenterPerformed.RemoveListener(GameInput_OnSenterPerformed);
        _gameInput.OnHealthPotionPerformed.RemoveListener(GameInput_OnHealthPotionPerformed);
    }

    public void ResetUnitCondition()
    {
        _isDead = false;
        _isBusy = false;
        _isKnocked = false;
        _isTakeDamageOnCooldown = false;
        _health.ResetHealthToMaximum();
        _shield.ResetShieldToMaximum();
        _healthPotion.ResetPotionToMax();
        _lantern.TurnOffLantern();
    }

    public void ApplyDamageToPlayer(bool enableKnockBack, Vector2 knockbackTargetPosition)
    {
        if (_gameStateService.CurrentState == GameState.BossDying)
            return;
        if (_isTakeDamageOnCooldown)
            return;
        if (enableKnockBack)
            _isKnocked = true;

        StartCoroutine(ApplyDamageToPlayerCoroutine(enableKnockBack, knockbackTargetPosition));
    }

    public void ApplyKnockBackToPlayer(Vector2 knockbackTargetPosition)
    {
        animator.SetTrigger("OnHit");
        StartCoroutine(HandleKnockBack(knockbackTargetPosition));
    }

    public void SetAnimationToIdleNoPan()
    {
        animator.Play("Idle_NoPan");
    }
    
    private void HandlePlayerAction()
    {
        if (_isKnocked)
            return;
        if (_isBusy)
            return;

        Vector2 inputVector = _gameInput.GetMovementVector();

        if (inputVector == Vector2.zero)
            return;
        if (IsInputVectorDiagonal(inputVector))
            return;

        _playerDir = inputVector;
        _lookOrientation.SetFacingDirection(_playerDir);
        
        if (Helper.CheckTargetDirection(transform.position, _playerDir, moveBlockMask, out Interactable interactable))
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
        _isBusy = true;

        animator.SetTrigger("Dash");
        PlayAudio(araAudioSO.Move);

        _moveTargetPosition = GetMoveTargetPosition();
        Helper.MoveToPosition(transform, _moveTargetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);

        _isBusy = false;
    }

    private IEnumerator HandleInteract(Interactable interactable)
    {
        _isBusy = true;

        switch (interactable)
        {
            case Pushable:
                animator.SetTrigger("Attack");
                interactable.Interact(this, _playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                _isBusy = false;
                yield break;
            case Damageable:
                animator.SetTrigger("Attack");
                interactable.Interact(this, _playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                _isBusy = false;
                yield break;
            case PillarLight:
                animator.SetTrigger("Attack");
                interactable.Interact(this, _playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                _isBusy = false;
                yield break;
            case Pickupable:
                interactable.Interact(this);
                yield return StartCoroutine(HandleMovement());
                _isBusy = false;
                yield break;
            default:
                break;
        }

        interactable.Interact(this, _playerDir);
        yield return Helper.GetWaitForSeconds(actionDuration);

        _isBusy = false;
    }  

    private IEnumerator ApplyDamageToPlayerCoroutine(bool knockBackPlayer, Vector2 knockbackTargetPosition)
    {
        _isTakeDamageOnCooldown = true;

        _health.TakeDamage();
        PlayAudio(araAudioSO.MoveBox[UnityEngine.Random.Range(0, 3)]);

        if (_health.CurrentHP <= 0)
            yield break;

        animator.SetTrigger("OnHit");
        damagedAnimator.Play("Ara_Damaged");
        _flashEffectController.PlayFlashEffect();

        if (knockBackPlayer)
        {
            StartCoroutine(HandleKnockBack(knockbackTargetPosition));
        }

        yield return Helper.GetWaitForSeconds(takeDamageCooldown);

        _isTakeDamageOnCooldown = false;
    }

    private IEnumerator HandleKnockBack(Vector2 targetPosition)
    {
        _isKnocked = true;

        _moveTargetPosition = targetPosition;
        Helper.MoveToPosition(transform, targetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);
        
        _isKnocked = false;
    }

    private void GameInput_OnSenterPerformed()
    {
        if (!_isLanternUnlocked)
            return;

        _lantern.ToggleLantern();
        PlayAudio(araAudioSO.Lantern);
    }

    private void GameInput_OnHealthPotionPerformed()
    {
        if (!_isHealthPotionUnlocked)
            return;
        if (_health.IsHealthFull())
            return;
        if (_healthPotion.CurrentPotionAmount <= 0) 
            return;
        if (_healthPotion.IsHealthPotionOnCooldown)
            return;

        _healthPotion.UsePotion();
        PlayAudio(araAudioSO.Potion);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    private Vector2 GetMoveTargetPosition()
    {
        var moveTargetPosition = (Vector2)transform.position + _playerDir;
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
