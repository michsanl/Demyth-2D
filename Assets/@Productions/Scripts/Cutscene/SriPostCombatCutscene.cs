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
    [SerializeField] private TalkableOnCollision _talkableOnCollision;
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
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

    private void Start()
    {
        _talkableOnCollision.OnDeathSlashCollision += Sri_OnDeathSlashCollision;
    }

    public void OnConversationEnd()
    {
        StartPostCombatCutscene();
    }

    private void Sri_OnDeathSlashCollision()
    {
        _dialogueSystemTrigger.OnUse();
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

        foreach (var item in _gameObjectsToActivate)
        {
            item.SetActive(true);
        }

        foreach (var item in _gameObjectsToDeactivate)
        {
            item.SetActive(false);
        }

        SaveSystem.SaveToSlot(1);

        _musicController.StopAllCoroutines();
        _musicController.EndSriBossFightMusic();
    }
}
