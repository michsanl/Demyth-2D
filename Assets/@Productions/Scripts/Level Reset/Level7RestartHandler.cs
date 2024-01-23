using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using DG.Tweening;
using Lean.Pool;
using Demyth.UI;

public class Level7RestartHandler : SceneService
{

    public Action<Action> OnPlayerDeathByBoss;
    public Action<Action> OnPlayerDeathByDialogue;

    [SerializeField] private SriCombatBehaviour _sriCombatBehaviour;
    [SerializeField] private GameObject _sriPreCombatCutscene;

    private Player _player;
    private Health _playerHealth;
    private GameStateService _gameStateService;
    private GameHUD _gameHUD;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
        _gameHUD = SceneServiceProvider.GetService<GameHUD>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void OnDisable() 
    {
        _playerHealth.OnDeath -= PlayerHealth_OnDeath;
    }

    public void PlayeDeathByDialogue()
    {
        _gameStateService.SetState(GameState.GameOver);
        LeanPool.DespawnAll();
        OnPlayerDeathByDialogue?.Invoke(RestartLevel);
    }

    private void PlayerHealth_OnDeath()
    {
        _gameStateService.SetState(GameState.GameOver);
        LeanPool.DespawnAll();
        OnPlayerDeathByBoss?.Invoke(RestartBossFight);
    }

    public void RestartBossFight()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _player.ResetUnitCondition();
        _sriPreCombatCutscene.SetActive(false);
        _sriCombatBehaviour.InitiateCombat();
    }

    public void RestartLevel()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _player.ResetUnitCondition();
        _gameHUD.Open();
    }
}
