using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Core;
using Demyth.Gameplay;

public class SriCombatBehaviorNew : MonoBehaviour
{

    private enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport }
    private enum CombatMode 
    { FirstPhase, SecondPhase, OldFirstPhase, AbilityLoop }


    [SerializeField] private bool _combatMode;
    [SerializeField] private int _phaseTwoHPThreshold;
    [SerializeField, EnumToggleButtons] private CombatMode _selectCombatMode;
    [SerializeField, EnumToggleButtons, Space] private Ability _abilityLoop;
    [SerializeField] private GameObject[] _attackColliderArray;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClipSriSO _sriAudioSO;

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

    
    private CombatMode _currentCombatMode;
    private PlayerManager _playerManager;
    private Player _player;
    private LookOrientation _lookOrientation;
    private Health _health;

    private int _meleeAbilityCounter;
    private int _rangeAbilityCount;

    private void Awake()
    {
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
        _health.OnTakeDamage += Health_OnTakeDamage;
        _health.OnDeath += Health_OnDeath;
    }

    private void Update()
    {
        UpdateCombatBehaviour();
    }

    private void OnEnable()
    {
        if (_combatMode)
        {
            ActivateSelectedCombatMode();
        }
    }

    private void UpdateCombatBehaviour()
    {
        if (!_combatMode)
            return;
        
        if (_currentCombatMode != _selectCombatMode)
        {
            _currentCombatMode = _selectCombatMode;
            ActivateSelectedCombatMode();
        }
    }

    private void Health_OnTakeDamage()
    {
        if (_health.CurrentHP == _phaseTwoHPThreshold)
        {
            StopCurrentAbility();
            StartCoroutine(StartPhaseTwo());
        }
    }

    private void Health_OnDeath()
    {
        StopCurrentAbility();
        StartCoroutine(StartDeathSlashAbility());
    }

    private void ActivateSelectedCombatMode()
    {
        StopCurrentAbility();

        switch (_selectCombatMode)
        {
            case CombatMode.FirstPhase:
                StartCoroutine(LoopCombatBehavior(GetFirstPhaseAbility));
                break;
            case CombatMode.SecondPhase:
                StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
                break;
            case CombatMode.OldFirstPhase:
                StartCoroutine(LoopCombatBehavior(GetOldFirstPhaseAbility));
                break;
            case CombatMode.AbilityLoop:
                StartCoroutine(LoopCombatBehavior(GetAbilityTesterAbility));
                break;
        }
    }

    private IEnumerator LoopCombatBehavior(Func<IEnumerator> getAbility)
    {
        if (_player.IsDead)
            yield break;

        IEnumerator ability = getAbility();
        SetFacingDirection();
        yield return StartCoroutine(ability);
        StartCoroutine(LoopCombatBehavior(getAbility));
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
        if (UnityEngine.Random.Range(0, 2) == 0)
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
            default:
                return null;
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

    private IEnumerator StartDeathSlashAbility()
    {
        yield return _deathSlashAbility.DeathSlash(_animator, _sriAudioSO.NailAOE, _sriAudioSO.VerticalSlash);
    }


    private IEnumerator StartPhaseTwo()
    {
        yield return StartCoroutine(StartWaveOutNailWaveAbility());
        StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
    }

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

    private void StopCurrentAbility()
    {
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
