using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Core;
using Demyth.Gameplay;

public class SriBossController : MonoBehaviour
{
    public Ability SelectedAbility => _selectedAbility;
    public Action OnPhaseTwoStart;

    public enum CombatMode 
    { None, FirstPhase, SecondPhase, ThirdPhase, AbilityTester }
    public enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport, DeathSlash }

    [SerializeField] private int _phaseTwoHPThreshold;
    [SerializeField] private int _phaseThreeHPThreshold;
    [SerializeField] private bool _combatTestMode;

    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private CombatMode _selectedCombatMode;
    [SerializeField, EnumToggleButtons, ShowIf("_combatTestMode")] 
    private Ability _selectedAbility;

    private GameStateService _gameStateService;
    private Health _health;
    private SriCombatModeController _combatModeController;
    private SriAbilityController _abilityContainer;
    private CombatMode _currentCombatMode;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _health = GetComponent<Health>();
        _abilityContainer = GetComponent<SriAbilityController>();
        _combatModeController = GetComponent<SriCombatModeController>();
    }

    private void Start()
    {
        _gameStateService[GameState.GameOver].onEnter += GameOver_OnEnter;
        _health.OnTakeDamage += Health_OnTakeDamage;
        _health.OnDeath += Health_OnDeath;

        if (_combatTestMode)
        {
            SwitchToPhase(_selectedCombatMode);
        }
    }

    private void Update()
    {
        if (_combatTestMode)
        {
            if (_currentCombatMode != _selectedCombatMode)
            {
                _currentCombatMode = _selectedCombatMode;
                SwitchToPhase(_selectedCombatMode);
            }
        }
    }

    public void InitiateCombat()
    {
        ResetUnitCondition();
        SwitchToPhase(CombatMode.FirstPhase);
    }

    private void Health_OnTakeDamage()
    {
        if (_health.CurrentHP == _phaseTwoHPThreshold)
        {
            SwitchToPhase(CombatMode.SecondPhase);
        }

        if (_health.CurrentHP == _phaseThreeHPThreshold)
        {
            SwitchToPhase(CombatMode.ThirdPhase);
        }
    }

    private void GameOver_OnEnter(GameState state)
    {
        _combatModeController.StopCurrentCombatModeLoop();
        _abilityContainer.StopCurrentAbility();
    }

    private void Health_OnDeath()
    {
        _gameStateService.SetState(GameState.BossDying);
        _combatModeController.StopCurrentCombatModeLoop();
        _abilityContainer.StopCurrentAbility();
        StartCoroutine(_abilityContainer.StartDeathSlashAbility());
    }

    private void SwitchToPhase(CombatMode selectedPhase)
    {
        _combatModeController.StopCurrentCombatModeLoop();
        _abilityContainer.StopCurrentAbility();

        switch (selectedPhase)
        {
            case CombatMode.None:
                break;
            case CombatMode.FirstPhase:
                _combatModeController.SwitchToFirstPhaseMode();
                break;
            case CombatMode.SecondPhase:
                OnPhaseTwoStart?.Invoke();
                _combatModeController.SwitchToSecondPhaseMode();
                break;
            case CombatMode.ThirdPhase:
                _combatModeController.SwitchToThirdPhaseMode();
                break;
            case CombatMode.AbilityTester:
                _combatModeController.SwitchToAbilityTesterMode();
                break;
        }
    }

    private void ResetUnitCondition()
    {
        _health.ResetHealthToMaximum();
        _combatModeController.StopCurrentCombatModeLoop();
        _abilityContainer.StopCurrentAbility();
    }
   
}
