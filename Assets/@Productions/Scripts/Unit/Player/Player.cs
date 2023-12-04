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
    public Action OnInvulnerableVisualStart;
    public Action OnInvulnerableVisualEnd;
    public Action<bool> OnSenterToggle;
    public Vector2 LastMoveTargetPosition => _moveTargetPosition;
    public Vector2 PlayerDir => _playerDir;
    public bool IsDead => _isDead;

    [Title("Settings")]
    [SerializeField] private float actionDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float takeDamageCooldown = 1f;
    [SerializeField] private LayerMask moveBlockMask;
    
    [Title("Components")]
    [SerializeField] private MMF_Player _hitFlashEffectMMFPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator damagedAnimator;
    [SerializeField] private AudioClipAraSO araAudioSO;
    [SerializeField] private GameObject senterGameObject;
    [SerializeField] private GameObject hitEffect;

    private LookOrientation _lookOrientation;
    private HealthPotion _healthPotion;
    private Health _health;
    private Shield _shield;
    private Vector2 _playerDir;
    private Vector2 _moveTargetPosition;
    private bool _isDead;
    private bool _isBusy;
    private bool _isKnocked;
    private bool _isTakeDamageOnCooldown;
    private bool _isSenterEnabled;
    private bool _isSenterUnlocked = true;
    private bool _isHealthPotionUnlocked = true;

    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        _lookOrientation = GetComponent<LookOrientation>();
        _healthPotion = GetComponent<HealthPotion>();
        _health = GetComponent<Health>();
        _shield = GetComponent<Shield>();

        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _gameInput = _gameInputController.GameInput;

        _health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath()
    {
        _isDead = true;
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

    public void ResetPlayerCondition()
    {
        _isDead = false;
        _isBusy = false;
        _isKnocked = false;
        _isTakeDamageOnCooldown = false;
        _health.ResetHealthToMaximum();
        _shield.ResetShieldToMaximum();
        _healthPotion.ResetPotionToMax();
        StartCoroutine(TurnOffSenter());
    }

    public void ApplyDamageToPlayer(bool enableKnockBack, Vector2 knockbackTargetPosition)
    {
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

        _hitFlashEffectMMFPlayer.PlayFeedbacks();
        animator.SetTrigger("OnHit");
        damagedAnimator.Play("Ara_Damaged");

        if (knockBackPlayer)
        {
            StartCoroutine(HandleKnockBack(knockbackTargetPosition));
        }

        yield return Helper.GetWaitForSeconds(takeDamageCooldown);

        _isTakeDamageOnCooldown = false;
    }

    private void GameInput_OnSenterPerformed()
    {
        if (!_isSenterUnlocked)
            return;

        ToggleSenter();
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

        PlayAudio(araAudioSO.Potion);
        _healthPotion.UsePotion();
    }

    private void ToggleSenter()
    {
        if (senterGameObject.activeInHierarchy)
        {
            StartCoroutine(TurnOffSenter());
        }
        else
        {
            StartCoroutine(TurnOnSenter());
        }
        PlayAudio(araAudioSO.Lantern);
    }

    private IEnumerator TurnOffSenter()
    {
        // Move the object away to trigger OnCollisonExit
        senterGameObject.transform.localPosition = new Vector3(100, 100, 0);
        yield return Helper.GetWaitForSeconds(0.05f);
        senterGameObject.SetActive(false);
        _isSenterEnabled = false;

        OnSenterToggle?.Invoke(_isSenterEnabled);
    }

    private IEnumerator TurnOnSenter()
    {
        senterGameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        yield return Helper.GetWaitForSeconds(0.05f);
        senterGameObject.SetActive(true);
        _isSenterEnabled = true;
        
        OnSenterToggle?.Invoke(_isSenterEnabled);
    }

    private IEnumerator HandleKnockBack(Vector2 targetPosition)
    {
        _isKnocked = true;
        _moveTargetPosition = targetPosition;
        Helper.MoveToPosition(transform, targetPosition, actionDuration);
        yield return Helper.GetWaitForSeconds(actionDuration);
        _isKnocked = false;
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
        _isBusy = false;
        _isKnocked = false;
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
