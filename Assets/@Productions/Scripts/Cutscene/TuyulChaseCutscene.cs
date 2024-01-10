using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TuyulChaseCutscene : MonoBehaviour
{
    [SerializeField] private TuyulChaseTalkable _yulaTalkable;
    [SerializeField] private TuyulChaseTalkable _yuliTalkable;
    [SerializeField] private GameObject _dialogueTrigger;
    [SerializeField] private GameObject _tuyulChaseLevelReset;
    [SerializeField] private GameObject _nextLevelGate;

    private GameStateService _gameStateService;
    private Player _player;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;

        _yulaTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
        _yuliTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
    }

    public void OnConversationEnd()
    {
        if (_gameStateService.CurrentState == GameState.MainMenu) return;
        
        _player.UsePan = false;
        _dialogueTrigger.SetActive(false);
        SaveSystem.SaveToSlot(1);
    }

    private void TuyulTalkable_OnAllTuyulCaught(GameObject sender)
    {
        CompleteLevel(sender);
    }

    private void CompleteLevel(GameObject sender)
    {
        // disable last active tuyul
        // give ara pan
        // save
        // disable level reset

        sender.SetActive(false);
        _nextLevelGate.SetActive(true);

        _player.UsePan = true;

        SaveSystem.SaveToSlot(1);

        _tuyulChaseLevelReset.SetActive(false);
    }
}
