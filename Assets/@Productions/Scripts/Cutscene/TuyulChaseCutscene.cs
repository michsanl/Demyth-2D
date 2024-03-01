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
    [SerializeField] private GameObject _tuyulChaseLevelReset;
    [SerializeField] private GameObject _prevLevelGate;
    [SerializeField] private GameObject _nextLevelGate;

    private Player _player;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;

        _yulaTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
        _yuliTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
    }

    private void TuyulTalkable_OnAllTuyulCaught(GameObject sender)
    {
        CompleteLevel(sender);
    }

    private void CompleteLevel(GameObject lastTuyul)
    {
        // disable last active tuyul
        // give ara pan
        // save
        // disable level reset

        lastTuyul.SetActive(false);
        _prevLevelGate.SetActive(true);
        _nextLevelGate.SetActive(true);
        _player.UsePan = true;

        SaveSystem.SaveToSlot(1);

        _tuyulChaseLevelReset.SetActive(false);
    }
}
