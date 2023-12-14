using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Core;
using Demyth.Gameplay;
using DG.Tweening;

public class PetraCombatBehaviour : MonoBehaviour
{
    
    private enum Ability 
    { UpCharge, DownCharge, HorizontalCharge, SpinAttack, ChargeAttack, BasicSlam, JumpSlam, JumpGroundSlam }
    private enum CombatMode 
    { None, FirstPhase, SecondPhase, AbilityLoop }

    [SerializeField] private bool _combatOnEnable;
    [SerializeField] private int _phaseTwoHPTreshold;
    [SerializeField, EnumToggleButtons] private CombatMode _selectedCombatMode;
    [SerializeField, EnumToggleButtons] private Ability _loopAbility;
    [SerializeField] private GameObject[] _attackColliderArray;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClipPetraSO _petraAudioSO;

    private PetraAbilityUpCharge _upChargeAbility;
    private PetraAbilityDownCharge _downChargeAbility;
    private PetraAbilityHorizontalCharge _horizontalChargeAbility;
    private PetraAbilitySpinAttack _spinAttackAbility;
    private PetraAbilityChargeAttack _chargeAttackAbility;
    private PetraAbilityBasicSlam _basicSlamAbility;
    private PetraAbilityJumpSlam _jumpSlamAbility;
    private PetraAbilityJumpGroundSlam _jumpGroundSlamAbility;

    private CombatMode _currentCombatMode;
    private PlayerManager _playerManager;
    private Player _player;
    private LookOrientation _lookOrientation;
    private Health _health;
    private int _lastRandomResult;
    private int _consecutiveCount;
    private float _timeVarianceCompensationDelay = 0.05f;

    private void Awake()
    {
        _playerManager = SceneServiceProvider.GetService<PlayerManager>();
        _lookOrientation = GetComponent<LookOrientation>();
        _health = GetComponent<Health>();
        _player = _playerManager.Player;

        _upChargeAbility = GetComponent<PetraAbilityUpCharge>();
        _downChargeAbility = GetComponent<PetraAbilityDownCharge>();
        _horizontalChargeAbility = GetComponent<PetraAbilityHorizontalCharge>();
        _spinAttackAbility = GetComponent<PetraAbilitySpinAttack>();
        _chargeAttackAbility = GetComponent<PetraAbilityChargeAttack>();
        _basicSlamAbility = GetComponent<PetraAbilityBasicSlam>();
        _jumpSlamAbility = GetComponent<PetraAbilityJumpSlam>();
        _jumpGroundSlamAbility = GetComponent<PetraAbilityJumpGroundSlam>();
    }

    private void Start()
    {
        _health.OnTakeDamage += Health_OnTakeDamage;
        _health.OnDeath += Health_OnDeath;
    }

    private void Update()
    {
        UpdateCombatMode();
    }

    private void OnEnable()
    {
        if (_combatOnEnable)
        {
            StartCoroutine(StartCombatWithIntroMove());
        }
    }

    public void InitiateCombatMode()
    {
        _health.ResetHealthToMaximum();
        DeactivateAllAttackCollider();
        StartCoroutine(StartCombatWithIntroMove());
    }

    public void ResetUnitCondition()
    {
        _selectedCombatMode = CombatMode.None;
        _health.ResetHealthToMaximum();
    }

    public void PlayReviveAnimation()
    {
        _animator.Play("Revive");
    }

    private IEnumerator StartCombatWithIntroMove()
    {
        yield return StartCoroutine(StartJumpSlamToMiddleArena());

        if (_selectedCombatMode == CombatMode.None)
        {
            _selectedCombatMode = CombatMode.FirstPhase;
        }
        else
        {
            ActivateSelectedCombatMode();
        }
    }

    private void UpdateCombatMode()
    {
        if (_currentCombatMode != _selectedCombatMode)
        {
            _currentCombatMode = _selectedCombatMode;
            ActivateSelectedCombatMode();
        }
    }

    private void Health_OnTakeDamage()
    {
        if (_health.CurrentHP == _phaseTwoHPTreshold)
        {
            StopCurrentAbility();
            StartCoroutine(StartPhaseTwo());
        }
    }

    private void Health_OnDeath()
    {
        StopCurrentAbility();
        _animator.Play("Defeated");
    }
    
    private void ActivateSelectedCombatMode()
    {
        StopCurrentAbility();

        switch (_selectedCombatMode)
        {
            case CombatMode.None:
                break;
            case CombatMode.FirstPhase:
                StartCoroutine(LoopAbility(GetFirstPhaseAbility));
                break;
            case CombatMode.SecondPhase:
                StartCoroutine(LoopAbility(GetSecondPhaseAbility));
                break;
            case CombatMode.AbilityLoop:
                StartCoroutine(LoopAbility(GetAbilityTesterAbility));
                break;
        }
    }

    private IEnumerator StartPhaseTwo()
    {
        yield return StartJumpGroundSlamAbility();
        yield return Helper.GetWaitForSeconds(_timeVarianceCompensationDelay);
    
        StartCoroutine(LoopAbility(GetSecondPhaseAbility));
    }

    private void StopCurrentAbility()
    {
        StopAllCoroutines();
        transform.DOKill();
        DeactivateAllAttackCollider();
    }


    private IEnumerator LoopAbility(Func<IEnumerator> selectedPhaseAbility)
    {
        if (_player.IsDead)
            yield break;

        IEnumerator ability = selectedPhaseAbility();

        SetFacingDirection();
        yield return StartCoroutine(ability);
        yield return Helper.GetWaitForSeconds(_timeVarianceCompensationDelay);

        StartCoroutine(LoopAbility(selectedPhaseAbility));
    }
    
    private IEnumerator GetFirstPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            return StartSpinAttackAbility();
        }

        if (IsPlayerInlineHorizontally())
        {
            return StartHorizontalChargeAbility();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? StartUpChargeAbility() : StartDownChargeAbility();
        }

        if (!IsPlayerNearby())
        {
            return StartBasicSlamAbility();
        }
        
        return null;
    }

    private IEnumerator GetSecondPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            return randomIndex == 0 ? StartSpinAttackAbility() : StartChargeAttackAbility();
        }

        if (IsPlayerInlineHorizontally())
        {
            return StartHorizontalChargeAbility();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? StartUpChargeAbility() : StartDownChargeAbility();
        }

        if (!IsPlayerNearby())
        {
            int random = GetRandomNumberWithConsecutiveLimit(1, 3, 3);
            return random == 1 ? StartJumpSlamAbility() : StartBasicSlamAbility();
        }

        return null;
    }

    private IEnumerator GetAbilityTesterAbility()
    {
        switch (_loopAbility)
        {
            case Ability.UpCharge:
                return StartUpChargeAbility();
            case Ability.DownCharge:
                return StartDownChargeAbility();
            case Ability.HorizontalCharge:
                return StartHorizontalChargeAbility();
            case Ability.SpinAttack:
                return StartSpinAttackAbility();
            case Ability.ChargeAttack:
                return StartChargeAttackAbility();
            case Ability.BasicSlam:
                return StartBasicSlamAbility();
            case Ability.JumpSlam:
                return StartJumpSlamAbility();
            case Ability.JumpGroundSlam:
                return StartJumpGroundSlamAbility();
            default:
                return null;
        }
    }

    private IEnumerator StartJumpGroundSlamAbility()
    {
        yield return _jumpGroundSlamAbility.JumpGroundSlam(_animator, _petraAudioSO.JumpSlam);
    }

    private IEnumerator StartUpChargeAbility()
    {
        yield return _upChargeAbility.UpCharge(_player, _animator, _petraAudioSO.RunCharge);
    }

    private IEnumerator StartDownChargeAbility()
    {
        yield return _downChargeAbility.DownCharge(_player, _animator, _petraAudioSO.RunCharge);
    }

    private IEnumerator StartHorizontalChargeAbility()
    {
        yield return _horizontalChargeAbility.HorizontalCharge(_player, _animator, _petraAudioSO.RunCharge);
    }

    private IEnumerator StartSpinAttackAbility()
    {
        yield return _spinAttackAbility.SpinAttack(_animator, _petraAudioSO.CoffinSwing);
    }
    
    private IEnumerator StartChargeAttackAbility()
    {
        yield return _chargeAttackAbility.ChargeAttack(_animator, _petraAudioSO.ChargeSlam);
    }
    
    private IEnumerator StartJumpSlamAbility()
    {
        yield return _jumpSlamAbility.JumpSlam(_player.LastMoveTargetPosition, _animator, _petraAudioSO.JumpSlam);
    }
    
    private IEnumerator StartJumpSlamToMiddleArena()
    {
        yield return _jumpSlamAbility.JumpSlam(new Vector2(0, -1), _animator, _petraAudioSO.JumpSlam);
    }
    
    private IEnumerator StartBasicSlamAbility()
    {
        yield return _basicSlamAbility.BasicSlam(_player, _animator, _petraAudioSO.BasicSlam);
    }


    private int GetRandomNumberWithConsecutiveLimit(int min, int max, int consecutiveLimit)
    {
        int random = UnityEngine.Random.Range(min, max);
        if (random == _lastRandomResult)
        {
            _consecutiveCount++;
        }
        else
        {
            _consecutiveCount = 0;
        }
        if (_consecutiveCount > consecutiveLimit)
        {
            while (random == _lastRandomResult)
            {
                random = UnityEngine.Random.Range(min, max);
            }
        }
        _lastRandomResult = random;
        return random;
    }

    private void DeactivateAllAttackCollider()
    {
        foreach (GameObject collider in _attackColliderArray)
        {
            collider.SetActive(false);
        }
    }

    protected void SetFacingDirection()
    {
        if (IsPlayerToRight())
        {
            _lookOrientation.SetFacingDirection(Vector2.right);
        }

        if (IsPlayerToLeft())
        {
            _lookOrientation.SetFacingDirection(Vector2.left);
        }
    }







#region PlayerToBossPositionInfo
    protected bool IsPlayerAbove()
    {
        return transform.position.y < _player.transform.position.y;
    }

    protected bool IsPlayerBelow()
    {
        return transform.position.y > _player.transform.position.y;
    }

    protected bool IsPlayerToRight()
    {
        return transform.position.x < _player.transform.position.x;
    }

    protected bool IsPlayerToLeft()
    {
        return transform.position.x > _player.transform.position.x;
    }

    protected bool IsPlayerInlineVertically()
    {
        return Mathf.Approximately(transform.position.x, _player.transform.position.x) ;
    }

    protected bool IsPlayerInlineHorizontally()
    {
        return Mathf.Approximately(transform.position.y, _player.transform.position.y);
    }

    protected bool IsPlayerNearby()
    {
        return Vector2.Distance(transform.position, _player.transform.position) < 1.5f;
    }
#endregion

}
