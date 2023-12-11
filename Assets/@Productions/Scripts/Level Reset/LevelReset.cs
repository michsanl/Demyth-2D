using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    [SerializeField] private int saveSlot;
    [SerializeField] private string levelUnlockConditionName;

    public static Action OnAnyRestartLevelEnabled;
    public static Action OnAnyRestartLevelDisabled;

    private GameInputController _gameInputController;
    private GameInput _gameInput;

    private void Awake()
    {
        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _gameInput = _gameInputController.GameInput;
    }

    private void OnEnable()
    {
        if (IsLevelCompleted())
            return;

        _gameInput.OnRestartPerformed.AddListener(GameInput_OnRestartPerformed);

        if (!SaveSystem.HasSavedGameInSlot(saveSlot))
        {
            SaveSystem.SaveToSlot(saveSlot);
        }

        OnAnyRestartLevelEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        _gameInput.OnRestartPerformed.RemoveListener(GameInput_OnRestartPerformed);

        OnAnyRestartLevelDisabled?.Invoke();
    }

    private void GameInput_OnRestartPerformed()
    {
        Debug.Log("restart performed");
        DOTween.CompleteAll();
        SaveSystem.LoadFromSlot(saveSlot);
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable(levelUnlockConditionName).asBool == true;
    }

    
}
