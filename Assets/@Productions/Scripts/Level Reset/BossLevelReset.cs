using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class BossLevelReset : MonoBehaviour
{


    [SerializeField] private GameObject _bossComabatModePrefab;
    [SerializeField] private GameObject _bossIdlePrefab;
    [SerializeField] private GameObject _invisibleDialogueTriggerPrefab;
    [Space]
    [SerializeField] private Vector3 _playerDefaultPosition;
    [SerializeField] private Vector3 _npcBossDefaultPosition;
    private Player _player;
    private Health _playerHealth;
    private Health _bossHealth;
    private GameStateService _gameStateService;
    
    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
        _bossHealth = _bossComabatModePrefab.GetComponent<Health>();
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
    }

    public void ResetLevel()
    {
        // To do :
        // Reset health, shield, etc on Player & Boss
        // Set back position on Player & Boss
        // Activate and deactivate objects

        _player.gameObject.SetActive(true);
        _player.ResetPlayerCondition();

        _bossHealth.ResetHealthToMaximum();

        ResetUnitPosition();
        ResetActiveState();
    }

    private void ResetUnitPosition()
    {
        _player.transform.position = _playerDefaultPosition;
        _bossComabatModePrefab.transform.position = _npcBossDefaultPosition;
    }

    private void ResetActiveState()
    {
        _bossIdlePrefab.SetActive(true);
        _invisibleDialogueTriggerPrefab.SetActive(true);
        
        _bossComabatModePrefab.SetActive(false);
    }
}
