using System;
using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using DG.Tweening;
using PixelCrushers;
using UnityEngine;

public class LevelReset : SceneService
{
    
    [SerializeField] private int saveSlot;

    public static Action OnAnyRestartLevelEnabled;
    public static Action OnAnyRestartLevelDisabled;

    private void OnEnable() 
    {
        Context.GameInput.OnRestartPerformed += GameInput_OnRestartPerformed;
        if (!SaveSystem.HasSavedGameInSlot(saveSlot))
        {
            SaveSystem.SaveToSlot(saveSlot);
        }

        OnAnyRestartLevelEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        Context.GameInput.OnRestartPerformed -= GameInput_OnRestartPerformed;

        OnAnyRestartLevelDisabled?.Invoke();
    }

    private void GameInput_OnRestartPerformed()
    {
        Debug.Log("restart performed");
        DOTween.KillAll();
        SaveSystem.LoadFromSlot(saveSlot);
    }

    
}
