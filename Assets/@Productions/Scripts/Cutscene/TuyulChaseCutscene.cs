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
    public static Action OnAnyTuyulLevelComplete;

    [SerializeField] private TuyulChaseTalkable _yulaTalkable;
    [SerializeField] private TuyulChaseTalkable _yuliTalkable;
    [SerializeField] private GameObject _dialogueTrigger;

    private Player _player;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;

        _yulaTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
        _yuliTalkable.OnAllTuyulHasBeenCaught += TuyulTalkable_OnAllTuyulCaught;
    }

    public void OnConversationEnd()
    {
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
        // disable level reset UI

        sender.SetActive(false);

        _player.UsePan = true;

        SaveSystem.SaveToSlot(1);

        OnAnyTuyulLevelComplete?.Invoke();
    }
}
