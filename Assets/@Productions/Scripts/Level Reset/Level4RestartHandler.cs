using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using DG.Tweening;
using Lean.Pool;
using System.Collections.Generic;
using System.Collections;

public class Level4RestartHandler : MonoBehaviour
{
    
    [SerializeField] private PetraCombatBehaviour _petraCombatBehaviour;
    [SerializeField] private GameObject _petraPreCombatCutscene;

    private Player _player;
    private Health _playerHealth;
    private GameStateService _gameStateService;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        _gameStateService[GameState.GameOver].onExit += GameOver_OnExit;
        _playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void OnDisable() 
    {
        _gameStateService[GameState.GameOver].onExit -= GameOver_OnExit;
        _playerHealth.OnDeath -= PlayerHealth_OnDeath;
    }

    private void GameOver_OnExit(GameState state)
    {
        RestartLevelBossFight();
    }

    private void PlayerHealth_OnDeath()
    {
        _gameStateService.SetState(GameState.GameOver);
        LeanPool.DespawnAll();
    }

    public void RestartLevelBossFight()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        
        _player.ResetUnitCondition();
        _petraPreCombatCutscene.SetActive(false);
        
        StartCoroutine(ActivateBossCombatMode());
    }
    
    private IEnumerator ActivateBossCombatMode()
    {
        yield return Helper.GetWaitForSeconds(.8f);
        _petraCombatBehaviour.InitiateCombat();
    }
}
