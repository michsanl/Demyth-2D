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
    public Action<bool> OnLanternValueChanged;
    public Action<bool> OnHealthPotionUnlockedValueChanged;
    public Action<bool> OnShieldUnlockedValueChanged;
    public Transform PlayerModel => _playerModel;
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
    public bool IsLanternUnlocked
    {
        get => _isLanternUnlocked;
        set
        {
            _isLanternUnlocked = value;
            OnLanternValueChanged?.Invoke(value);
        }
    }
    public bool IsHealthPotionUnlocked
    {
        get => _isHealthPotionUnlocked;
        set
        {
            _isHealthPotionUnlocked = value;
            OnHealthPotionUnlockedValueChanged?.Invoke(value);
        }
    }
    public bool IsShieldUnlocked
    {
        get => _isShieldUnlocked;
        set
        {
            _isShieldUnlocked = value;
            OnShieldUnlockedValueChanged?.Invoke(value);
        }
    }

    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private Transform _playerModel;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private AraClipSO _araClipSO;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    private GameStateService _gameStateService;
    private GameInput _gameInput;
    private LevelManager _levelManager;
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
    private bool _isLanternUnlocked;
    private bool _isHealthPotionUnlocked;
    private bool _isShieldUnlocked;


    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameInput = SceneServiceProvider.GetService<GameInputController>().GameInput;
        _levelManager = SceneServiceProvider.GetService<LevelManager>();
        _lookOrientation = GetComponent<LookOrientation>();
        _healthPotion = GetComponent<HealthPotion>();
        _health = GetComponent<Health>();
        _shield = GetComponent<Shield>();
        _lantern = GetComponent<Lantern>();
        _flashEffectController = GetComponent<FlashEffectController>();

    }

    private void Start()
    {
        Signaler.Instance.Broadcast(this, new PlayerSpawnEvent { Player = gameObject });
        
        if (_gameSettingsSO != null)
        {
            UsePan = _gameSettingsSO.UsePanOnStart;
            IsLanternUnlocked = _gameSettingsSO.UnlockLanternOnStart;
            IsHealthPotionUnlocked = _gameSettingsSO.UnlockPotionOnStart;
            IsShieldUnlocked = _gameSettingsSO.UnlockShieldOnStart;
        }
    }

    private void Update()
    {
        HandlePlayerAction();
    }

    private void OnEnable()
    {
        ResetUnitCondition();

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
        
        UsePan = _usePan;
        _moveTargetPosition = transform.position + Vector3.up;
    }

    public void UnlockLantern()
    {
        IsLanternUnlocked = true;
    }

    public void UnlockPotion()
    {
        IsHealthPotionUnlocked = true;
    }

    public void UnlockShield()
    {
        IsShieldUnlocked = true;
    }

    public bool ApplyDamageToPlayer(bool enableKnockBack, Vector2 knockbackTargetPosition)
    {
        if (_gameStateService.CurrentState == GameState.BossDying) return false;
        if (_isTakeDamageOnCooldown) return false;

        if (enableKnockBack)
        {
            _isKnocked = true;
        }

        StartCoroutine(ApplyDamageToPlayerCoroutine(enableKnockBack, knockbackTargetPosition));
        return true;
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
        PlayAudio(_araClipSO.Move, _araClipSO.MoveVolume);

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
                PlayAttackHitAudio();
                interactable.Interact(this, _playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                _isBusy = false;
                yield break;
            case Damageable:
                animator.SetTrigger("Attack");
                PlayAttackHitAudio();
                interactable.Interact(this, _playerDir);
                yield return Helper.GetWaitForSeconds(attackDuration);
                _isBusy = false;
                yield break;
            case PillarLight:
                animator.SetTrigger("Attack");
                PlayAttackHitAudio();
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

        TakeDamage();

        if (_health.CurrentHP <= 0) yield break;

        animator.SetTrigger("OnHit");
        damagedAnimator.Play("Ara_Damaged");
        // _flashEffectController.PlayFlashEffect();

        if (knockBackPlayer) StartCoroutine(HandleKnockBack(knockbackTargetPosition));

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

    private void TakeDamage()
    {
        if (_isShieldUnlocked)
        {
            if (_shield.TryShieldTakeDamage()) 
            {
                return;
            }
            else 
            {
                _health.TakeDamage();
            }
        }
        else
        {
            _health.TakeDamage();
        }
    }

    private void GameInput_OnSenterPerformed()
    {
        if (!_isLanternUnlocked)
            return;

        _lantern.ToggleLantern();
        PlayAudio(_araClipSO.Lantern, _araClipSO.LanternVolume);
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
        PlayAudio(_araClipSO.Potion, _araClipSO.PotionVolume);
    }

    private void PlayAttackHitAudio()
    {
        int random;

        if (_usePan)
        {
            random = UnityEngine.Random.Range(0, _araClipSO.PanHit.Length);
            PlayAudio(_araClipSO.PanHit[random], _araClipSO.GetPanHitVolume(random));
        }
        else
        {
            random = UnityEngine.Random.Range(0, _araClipSO.MoveBox.Length);
            PlayAudio(_araClipSO.MoveBox[random], _araClipSO.GetMoveBoxVolume(random));
        }
    }

    private void PlayAudio(AudioClip abilitySFX , float volume)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = volume;
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
