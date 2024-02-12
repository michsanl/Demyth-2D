using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Core;
using Demyth.Gameplay;
using System;

public class SriCombatModeController : MonoBehaviour
{

    public Action OnPhaseTwoStarted;

    private Coroutine _currentPhaseLoopCoroutine;
    private Player _player;
    private LookOrientation _lookOrientation;
    private AbilitySelector _firstPhaseAbilitySelector;
    private AbilitySelector _secondPhaseAbilitySelector;
    private AbilitySelector _thirdPhaseAbilitySelector;
    private AbilitySelector _abilityTesterAbilitySelector;
    private SriAbilityController _abilityContainer;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _lookOrientation = GetComponent<LookOrientation>();
        _abilityContainer = GetComponent<SriAbilityController>();
        _firstPhaseAbilitySelector = GetComponent<SriFirstPhase>();
        _secondPhaseAbilitySelector = GetComponent<SriSecondPhase>();
        _thirdPhaseAbilitySelector = GetComponent<SriThirdPhase>();
        _abilityTesterAbilitySelector = GetComponent<SriAbilityTester>();
    }

    public void SwitchToFirstPhaseMode()
    {
        _currentPhaseLoopCoroutine = StartCoroutine(PerformFirstPhaseLoop());
    }

    public void SwitchToSecondPhaseMode()
    {
        _currentPhaseLoopCoroutine = StartCoroutine(PerformSecondPhaseLoop());
    }

    public void SwitchToThirdPhaseMode()
    {
        _currentPhaseLoopCoroutine = StartCoroutine(PerformThirdPhaseLoop());
    }

    public void SwitchToAbilityTesterMode()
    {
        _currentPhaseLoopCoroutine = StartCoroutine(PerformAbilityTesterLoop());
    }

    public void StopCurrentCombatModeLoop()
    {
        if (_currentPhaseLoopCoroutine != null) StopCoroutine(_currentPhaseLoopCoroutine);
    }

    private IEnumerator PerformFirstPhaseLoop()
    {
        yield return _abilityContainer.StartTeleportToMiddleArena();

        while (true)
        {
            if (_player.IsDead) yield break;
            SetFacingDirectionToPlayer();
            yield return _firstPhaseAbilitySelector.GetAbility();
        }
    }

    private IEnumerator PerformSecondPhaseLoop()
    {
        yield return _abilityContainer.StartWaveOutNailWaveAbility();
        
        while (true)
        {
            if (_player.IsDead) yield break;
            SetFacingDirectionToPlayer();
            yield return  _secondPhaseAbilitySelector.GetAbility();
        }
    }

    private IEnumerator PerformThirdPhaseLoop()
    {
        yield return _abilityContainer.StartWaveOutNailWaveAbility();
        
        while (true)
        {
            if (_player.IsDead) yield break;
            SetFacingDirectionToPlayer();
            yield return _thirdPhaseAbilitySelector.GetAbility();;
        }
    }

    private IEnumerator PerformAbilityTesterLoop()
    {
        while (true)
        {
            if (_player.IsDead) yield break;
            SetFacingDirectionToPlayer();
            yield return _abilityTesterAbilitySelector.GetAbility();
        }
    }

    private void SetFacingDirectionToPlayer()
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

    private bool IsPlayerToRight()
    {
        return transform.position.x < _player.transform.position.x;
    }

    private bool IsPlayerToLeft()
    {
        return transform.position.x > _player.transform.position.x;
    }
}
