using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Core;
using Demyth.Gameplay;
using DG.Tweening;

public class SriCombatBehaviour : MonoBehaviour
{

    private enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport, DeathSlash }
    private enum CombatMode 
    { None, FirstPhase, SecondPhase, OldFirstPhase, AbilityLoop }


    [SerializeField] private int _phaseTwoHPThreshold;
    [SerializeField] private bool _combatTestMode;
    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private CombatMode _selectedCombatMode;
    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private Ability _abilityLoop;
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClipSriSO _sriAudioSO;
    [SerializeField] private GameObject[] _attackColliderArray;

    private SriAbilityUpSlash _upSlashAbility;
    private SriAbilityDownSlash _downSlashAbility;
    private SriAbilityHorizontalSlash _horizontalSlashAbility;
    private SriAbilitySpinClaw _spinClawAbility;
    private SriAbilityNailAOE _nailAOEAbility;
    private SriAbilityNailSummon _nailSummonAbility;
    private SriAbilityFireBall _fireBallAbility;
    private SriAbilityTeleport _teleportAbility;
    private SriAbilityHorizontalNailWave _horizontalNailWaveAbility;
    private SriAbilityVerticalNailWave _verticalNailWaveAbility;
    private SriAbilityWaveOutNailWave _waveOutNailWaveAbility;
    private SriAbilityDeathSlash _deathSlashAbility;

    private GameStateService _gameStateService;
    private CombatMode _currentCombatMode;
    private PlayerManager _playerManager;
    private Player _player;
    private LookOrientation _lookOrientation;
    private Health _health;

    private int _meleeAbilityCounter;
    private int _rangeAbilityCount;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _playerManager = SceneServiceProvider.GetService<PlayerManager>();
        _lookOrientation = GetComponent<LookOrientation>();
        _health = GetComponent<Health>();
        _player = _playerManager.Player;

        _upSlashAbility = GetComponent<SriAbilityUpSlash>();
        _downSlashAbility = GetComponent<SriAbilityDownSlash>();
        _horizontalSlashAbility = GetComponent<SriAbilityHorizontalSlash>();
        _spinClawAbility = GetComponent<SriAbilitySpinClaw>();
        _nailAOEAbility = GetComponent<SriAbilityNailAOE>();
        _nailSummonAbility = GetComponent<SriAbilityNailSummon>();
        _fireBallAbility = GetComponent<SriAbilityFireBall>();
        _teleportAbility = GetComponent<SriAbilityTeleport>();
        _horizontalNailWaveAbility = GetComponent<SriAbilityHorizontalNailWave>();
        _verticalNailWaveAbility = GetComponent<SriAbilityVerticalNailWave>();
        _waveOutNailWaveAbility = GetComponent<SriAbilityWaveOutNailWave>();
        _deathSlashAbility = GetComponent<SriAbilityDeathSlash>();
    }

    private void Start()
    {
        _gameStateService[GameState.GameOver].onEnter += GameOver_OnEnter;
        _health.OnTakeDamage += Health_OnTakeDamage;
        _health.OnDeath += Health_OnDeath;

        if (_combatTestMode)
        {
            ActivateSelectedCombatMode();
        }
    }

    private void Update()
    {
        if (!_combatTestMode) return;

        if (_currentCombatMode != _selectedCombatMode)
        {
            _currentCombatMode = _selectedCombatMode;
            ActivateSelectedCombatMode();
        }
    }

    public void InitiateCombat()
    {
        ResetUnitCondition();
        StartCoroutine(StartPhaseOne());
    }

    private IEnumerator StartPhaseOne()
    {
        yield return StartCoroutine(StartTeleportToMiddleArena());
        StartCoroutine(LoopCombatBehaviour(GetFirstPhaseAbility));
    }

    private IEnumerator StartPhaseTwo()
    {
        yield return StartCoroutine(StartWaveOutNailWaveAbility());
        StartCoroutine(LoopCombatBehaviour(GetSecondPhaseAbility));
    }

    private void Health_OnTakeDamage()
    {
        if (_health.CurrentHP == _phaseTwoHPThreshold)
        {
            StopAbility();
            StartCoroutine(StartPhaseTwo());
        }
    }

    private void GameOver_OnEnter(GameState state)
    {
        StopAbility();
    }

    private void Health_OnDeath()
    {
        _gameStateService.SetState(GameState.BossDying);
        StopAbility();

        StartCoroutine(StartDeathSlashAbility());
    }

    private void ResetUnitCondition()
    {
        _health.ResetHealthToMaximum();
        DeactivateAllAttackCollider();
    }

    private void StopAbility()
    {
        transform.DOKill();
        StopAllCoroutines();
        DeactivateAllAttackCollider();
    }

    private void DeactivateAllAttackCollider()
    {
        foreach (GameObject collider in _attackColliderArray)
        {
            collider.SetActive(false);
        }
    }

    ///////////////////////////// Combat Mode Loop /////////////////////////////

    private void ActivateSelectedCombatMode()
    {
        StopAbility();

        switch (_selectedCombatMode)
        {
            case CombatMode.None:
                break;
            case CombatMode.FirstPhase:
                StartCoroutine(LoopCombatBehaviour(GetFirstPhaseAbility));
                break;
            case CombatMode.SecondPhase:
                StartCoroutine(LoopCombatBehaviour(GetSecondPhaseAbility));
                break;
            case CombatMode.OldFirstPhase:
                StartCoroutine(LoopCombatBehaviour(GetOldFirstPhaseAbility));
                break;
            case CombatMode.AbilityLoop:
                StartCoroutine(LoopCombatBehaviour(GetAbilityTesterAbility));
                break;
        }
    }

    private IEnumerator LoopCombatBehaviour(Func<IEnumerator> getAbility)
    {
        if (_player.IsDead)
            yield break;

        IEnumerator ability = getAbility();
        SetFacingDirection();
        yield return StartCoroutine(ability);
        StartCoroutine(LoopCombatBehaviour(getAbility));
    }
    
    private IEnumerator GetOldFirstPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            IncreaseMeleeAbilityCounter();
            return _meleeAbilityCounter == 0 ? StartNailAOEAbility() : StartSpinClawAbility();
        }

        if (IsPlayerInlineHorizontally())
        {
            return StartHorizontalSlashAbility();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? StartUpSlashAbility() : StartDownSlashAbility();
        }

        if (!IsPlayerNearby())
        {
            return StartNailSummonAbility();
        }

        return null;
    }

    private IEnumerator GetFirstPhaseAbility()
    {
        if (UnityEngine.Random.Range(0, 3) == 0)
        {
            return StartTeleportAbility();
        }

        if (IsPlayerNearby())
        {
            _rangeAbilityCount = 0;
            IncreaseMeleeAbilityCounter();
            return _meleeAbilityCounter == 0 ? StartSpinClawAbility() : StartNailAOEAbility();
        }

        if (IsPlayerInlineHorizontally())
        {
            _rangeAbilityCount = 0;
            return StartHorizontalSlashAbility();
        }

        if (IsPlayerInlineVertically())
        {
            _rangeAbilityCount = 0;
            return IsPlayerAbove() ? StartUpSlashAbility() : StartDownSlashAbility();
        }

        if (!IsPlayerNearby())
        {
            _rangeAbilityCount++;
            if (_rangeAbilityCount > 5)
            {
                _rangeAbilityCount = 0;
                return StartTeleportAbility();
            }

            int randomIndex = UnityEngine.Random.Range(0, 2);
            return randomIndex == 0 ? StartFireBallAbility() : StartNailSummonAbility();
        }

        return null;
    }

    private IEnumerator GetSecondPhaseAbility()
    {
        return TeleportIntoNailWaveVariant();
    }

    private IEnumerator GetAbilityTesterAbility()
    {
        switch (_abilityLoop)
        {
            case Ability.UpSlash:
                return StartUpSlashAbility();
            case Ability.DownSlash:
                return StartDownSlashAbility();
            case Ability.HorizontalSlash:
                return StartHorizontalSlashAbility();
            case Ability.SpinClaw:
                return StartSpinClawAbility();
            case Ability.NailAOE:
                return StartNailAOEAbility();
            case Ability.NailSummon:
                return StartNailSummonAbility();
            case Ability.FireBall:
                return StartFireBallAbility();
            case Ability.HorizontalNailWave:
                return StartHorizontalNailWaveAbility();
            case Ability.VerticalNailWave:
                return StartVerticalNailWaveAbility();
            case Ability.WaveOutNailWave:
                return StartWaveOutNailWaveAbility();
            case Ability.Teleport:
                return StartTeleportAbility();
            case Ability.DeathSlash:
                return StartDeathSlashAbility();
            default:
                return null;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void IncreaseMeleeAbilityCounter()
    {
        _meleeAbilityCounter++;
        if (_meleeAbilityCounter > 2)
        {
            _meleeAbilityCounter = 0;
        }
    }

    private IEnumerator TeleportIntoNailWaveVariant()
    {
        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(StartTeleportAbility());            
            SetFacingDirection();
        }

        int randomNumber = UnityEngine.Random.Range(0, 2);
        switch (randomNumber)
        {
            case 0:
                yield return StartCoroutine(StartHorizontalNailWaveAbility());
                break;
            case 1:
                yield return StartCoroutine(StartVerticalNailWaveAbility());
                break;
            default:
                break;
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


    private IEnumerator StartUpSlashAbility()
    {
        yield return _upSlashAbility.UpSlash(_player, _animator, _sriAudioSO.VerticalSlash);
    }

    private IEnumerator StartDownSlashAbility()
    {
        yield return _downSlashAbility.DownSlash(_player, _animator, _sriAudioSO.VerticalSlash);
    }

    private IEnumerator StartHorizontalSlashAbility()
    {
        yield return _horizontalSlashAbility.HorizontalSlash(_player, _animator, _sriAudioSO.HorizontalSlash);
    }

    private IEnumerator StartSpinClawAbility()
    {
        yield return _spinClawAbility.SpinClaw(_animator, _sriAudioSO.SpinClaw);
    }

    private IEnumerator StartNailAOEAbility()
    {
        yield return _nailAOEAbility.NailAOE(_animator, _sriAudioSO.NailAOE);
    }

    private IEnumerator StartNailSummonAbility()
    {
        yield return _nailSummonAbility.NailSummon(_player, _animator, _sriAudioSO.NailSummon);
    }

    private IEnumerator StartFireBallAbility()
    {
        yield return _fireBallAbility.FireBall(_animator, _sriAudioSO.Fireball);
    }

    private IEnumerator StartHorizontalNailWaveAbility()
    {
        yield return _horizontalNailWaveAbility.HorizontalNailWave(_animator, _sriAudioSO.NailAOE);
    }

    private IEnumerator StartVerticalNailWaveAbility()
    {
        yield return _verticalNailWaveAbility.VerticalNailWave(_animator, _sriAudioSO.NailAOE);
    }

    private IEnumerator StartWaveOutNailWaveAbility()
    {
        yield return _waveOutNailWaveAbility.WaveOutNailWave(_animator, _sriAudioSO.NailAOE);
    }

    private IEnumerator StartTeleportAbility()
    {
        yield return _teleportAbility.Teleport(_player, _animator);
    }

    private IEnumerator StartTeleportToMiddleArena()
    {
        yield return _teleportAbility.Teleport(new Vector3(0, 1, 0), _animator);
    }

    private IEnumerator StartDeathSlashAbility()
    {
        yield return _deathSlashAbility.DeathSlash(_animator, _sriAudioSO.NailAOE, _sriAudioSO.VerticalSlash);
    }

#region Position to Player Checker

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
