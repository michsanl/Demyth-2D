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
    public Action OnPhaseTwoStart;
    public Action OnPhaseThreeStart;

    private enum SelectedCombatMode 
    { None, FirstPhase, SecondPhase, OldFirstPhase, AbilityLoop }
    private enum SelectedAbility
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport, DeathSlash }

    [SerializeField] private int _phaseTwoHPThreshold;
    [SerializeField] private int _phaseThreeHPThreshold;
    [SerializeField] private bool _combatTestMode;
    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private SelectedCombatMode _selectedCombatMode;
    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private SelectedAbility _selectedAbility;
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _attackColliderArray;

    private Ability _upSlash;
    private Ability _downSlash;
    private Ability _horizontalSlash;
    private Ability _spinClaw;
    private Ability _nailAOE;
    private Ability _nailSummon;
    private Ability _fireBall;
    private Ability _teleport;
    private Ability _teleportMiddleArena;
    private Ability _horizontalNailWave;
    private Ability _verticalNailWave;
    private Ability _waveOutNailWave;
    private Ability _deathSlash;

    private GameStateService _gameStateService;
    private SelectedCombatMode _currentCombatMode;
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

        _upSlash = GetComponent<SriAbilityUpSlash>();
        _downSlash = GetComponent<SriAbilityDownSlash>();
        _horizontalSlash = GetComponent<SriAbilityHorizontalSlash>();
        _spinClaw = GetComponent<SriAbilitySpinClaw>();
        _nailAOE = GetComponent<SriAbilityNailAOE>();
        _nailSummon = GetComponent<SriAbilityNailSummon>();
        _fireBall = GetComponent<SriAbilityFireBall>();
        _teleport = GetComponent<SriAbilityTeleport>();
        _teleportMiddleArena = GetComponent<SriTeleportToMiddleArena>();
        _horizontalNailWave = GetComponent<SriAbilityHorizontalNailWave>();
        _verticalNailWave = GetComponent<SriAbilityVerticalNailWave>();
        _waveOutNailWave = GetComponent<SriAbilityWaveOutNailWave>();
        _deathSlash = GetComponent<SriAbilityDeathSlash>();
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
        OnPhaseTwoStart?.Invoke();
        yield return StartCoroutine(StartWaveOutNailWaveAbility());
        StartCoroutine(LoopCombatBehaviour(GetFirstPhaseAbility));
    }

    private IEnumerator StartPhaseThree()
    {
        OnPhaseThreeStart?.Invoke();
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

        if (_health.CurrentHP == _phaseThreeHPThreshold)
        {
            StopAbility();
            StartCoroutine(StartPhaseThree());
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

        StartCoroutine(PlayDeathSlashAbility());
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
            case SelectedCombatMode.None:
                break;
            case SelectedCombatMode.FirstPhase:
                StartCoroutine(LoopCombatBehaviour(GetFirstPhaseAbility));
                break;
            case SelectedCombatMode.SecondPhase:
                StartCoroutine(LoopCombatBehaviour(GetSecondPhaseAbility));
                break;
            case SelectedCombatMode.OldFirstPhase:
                StartCoroutine(LoopCombatBehaviour(GetOldFirstPhaseAbility));
                break;
            case SelectedCombatMode.AbilityLoop:
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
        switch (_selectedAbility)
        {
            case SelectedAbility.UpSlash:
                return StartUpSlashAbility();
            case SelectedAbility.DownSlash:
                return StartDownSlashAbility();
            case SelectedAbility.HorizontalSlash:
                return StartHorizontalSlashAbility();
            case SelectedAbility.SpinClaw:
                return StartSpinClawAbility();
            case SelectedAbility.NailAOE:
                return StartNailAOEAbility();
            case SelectedAbility.NailSummon:
                return StartNailSummonAbility();
            case SelectedAbility.FireBall:
                return StartFireBallAbility();
            case SelectedAbility.HorizontalNailWave:
                return StartHorizontalNailWaveAbility();
            case SelectedAbility.VerticalNailWave:
                return StartVerticalNailWaveAbility();
            case SelectedAbility.WaveOutNailWave:
                return StartWaveOutNailWaveAbility();
            case SelectedAbility.Teleport:
                return StartTeleportAbility();
            case SelectedAbility.DeathSlash:
                return PlayDeathSlashAbility();
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
        yield return _upSlash.PlayAbility();
    }

    private IEnumerator StartDownSlashAbility()
    {
        yield return _downSlash.PlayAbility();
    }

    private IEnumerator StartHorizontalSlashAbility()
    {
        yield return _horizontalSlash.PlayAbility();
    }

    private IEnumerator StartSpinClawAbility()
    {
        yield return _spinClaw.PlayAbility();
    }

    private IEnumerator StartNailAOEAbility()
    {
        yield return _nailAOE.PlayAbility();
    }

    private IEnumerator StartNailSummonAbility()
    {
        yield return _nailSummon.PlayAbility();
    }

    private IEnumerator StartFireBallAbility()
    {
        yield return _fireBall.PlayAbility();
    }

    private IEnumerator StartHorizontalNailWaveAbility()
    {
        yield return _horizontalNailWave.PlayAbility();
    }

    private IEnumerator StartVerticalNailWaveAbility()
    {
        yield return _verticalNailWave.PlayAbility();
    }

    private IEnumerator StartWaveOutNailWaveAbility()
    {
        yield return _waveOutNailWave.PlayAbility();
    }

    private IEnumerator StartTeleportAbility()
    {
        yield return _teleport.PlayAbility();;
    }

    private IEnumerator StartTeleportToMiddleArena()
    {
        yield return _teleportMiddleArena.PlayAbility();
    }

    private IEnumerator PlayDeathSlashAbility()
    {
        yield return _deathSlash.PlayAbility();
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
