using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using DG.Tweening;
using Lean.Pool;

public class Level4RestartHandler : MonoBehaviour
{
    
    [SerializeField] private PetraCombatBehaviour _petraCombatBehaviour;

    private Player _player;
    private Health _playerHealth;
    private GameStateService _gameStateService;
    private CameraController _cameraController;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
        _cameraController = SceneServiceProvider.GetService<CameraController>();
    }

    private void OnEnable()
    {
        _gameStateService[GameState.Gameplay].onEnter += GameStateGamePlay_OnEnter;
        _playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void OnDisable() 
    {
        _gameStateService[GameState.Gameplay].onEnter -= GameStateGamePlay_OnEnter;
        _playerHealth.OnDeath -= PlayerHealth_OnDeath;
    }

    private void GameStateGamePlay_OnEnter(GameState state)
    {
        if (_gameStateService.PreviousState == GameState.GameOver)
        {
            ResetLevel();
        }
    }

    private void PlayerHealth_OnDeath()
    {
        _gameStateService.SetState(GameState.GameOver);
        LeanPool.DespawnAll();
    }

    public void ResetLevel()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _player.ResetUnitCondition();
        _petraCombatBehaviour.InitiateCombat();
        _cameraController.MoveCameraDownSmoothly();
    }
}
