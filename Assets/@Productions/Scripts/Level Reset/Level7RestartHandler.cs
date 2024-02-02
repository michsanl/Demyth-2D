using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using DG.Tweening;
using Lean.Pool;
using Demyth.UI;
using System.Collections;

public class Level7RestartHandler : MonoBehaviour
{

    [SerializeField] private SriCombatBehaviour _sriCombatBehaviour;
    [SerializeField] private GameObject _sriPreCombatCutscene;

    private Player _player;
    private Health _playerHealth;
    private GameStateService _gameStateService;
    private GameHUD _gameHUD;
    private DeathDescriptionManager _deathDescriptionManager;
    private bool _isPlayerDead;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
        _gameHUD = SceneServiceProvider.GetService<GameHUD>();
        _deathDescriptionManager = SceneServiceProvider.GetService<DeathDescriptionManager>();
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
        if (_isPlayerDead)
        {
            RestartLevelBossFight();
        }
        else
        {
            RestartLevelDialogue();
        }
    }

    private void PlayerHealth_OnDeath()
    {
        _isPlayerDead = true;
        LeanPool.DespawnAll();
        _deathDescriptionManager.SetDeathDescSriFight();

        _gameStateService.SetState(GameState.GameOver);
    }

    private void RestartLevelDialogue()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _player.ResetUnitCondition();
        _gameHUD.Open();
    }

    private void RestartLevelBossFight()
    {
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(1);
        _sriPreCombatCutscene.SetActive(false);
        _player.ResetUnitCondition();
        _isPlayerDead = false;
        StartCoroutine(ActivateBossCombatMode());
    }
    
    private IEnumerator ActivateBossCombatMode()
    {
        yield return Helper.GetWaitForSeconds(.9f);
        _sriCombatBehaviour.InitiateCombat();
    }
}
