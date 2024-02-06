using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using PixelCrushers;
using Core;
using Demyth.Gameplay;
using System;
using PixelCrushers.DialogueSystem;

public class SriPostCombatCutscene : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsToActivate;
    [SerializeField] private GameObject[] _gameObjectsToDeactivate;
    
    private GameStateService _gameStateService;
    private MusicController _musicController;
    private Player _player;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _musicController = SceneServiceProvider.GetService<MusicController>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public void StartPostCombatCutscene()
    {
        if (_gameStateService.CurrentState == GameState.MainMenu) return;

        // SEQUENCE 1
        // Activate end game NPC and portal
        // Deactivate summoned object
        // Deactivate boss sri
        // Deactivate invisible wall
        // Deactivate NPC on all previous floor
        // Save

        LeanPool.DespawnAll();
        _gameStateService.SetState(GameState.Gameplay);
        _player.ResetUnitCondition();

        foreach (var gameObject in _gameObjectsToActivate)
        {
            gameObject.SetActive(true);
        }

        foreach (var gameObject in _gameObjectsToDeactivate)
        {
            gameObject.SetActive(false);
        }

        SaveSystem.SaveToSlot(1);

        _musicController.StopAllCoroutines();
        _musicController.EndSriBossFightMusic();
    }
}
