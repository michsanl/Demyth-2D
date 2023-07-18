using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomTools.Core;
using UISystem;
using DG.Tweening;

public class GameManager : SceneService
{
    public Action OnGameStart;
    public Action OnOpenMainMenu;
    public Action OnGamePaused;
    public Action OnGameUnpaused;
    public bool IsGamePaused => isGamePaused;



    private bool isGamePaused;
    private bool isMainMenuOpen = true;

    protected override void OnInitialize()
    {

    }

    protected override void OnActivate()
    {
        Context.GameInput.OnPausePerformed += GameInput_OnPausePerformed;
    }

    private void GameInput_OnPausePerformed()
    {
        TogglePauseGame();
    }

    public void StartGame(string levelID)
    {
        Context.LevelManager.CurrentLevel.MoveToNextLevel(levelID);
        Context.UI.Close<MainMenuUI>();
        isMainMenuOpen = false;
        Context.Player.ActivatePlayer();

        OnGameStart?.Invoke();
    }

    public void GoToMainMenu()
    {
        Context.LevelManager.OpenMainMenuLevel();
        Context.UI.Open<MainMenuUI>();
        isMainMenuOpen = true;

        OnOpenMainMenu?.Invoke();

        Context.CameraNormal.transform.DOKill();
        Context.CameraNormal.transform.localPosition = Vector3.zero;
    }

    public void TogglePauseGame()
    {
        if (isMainMenuOpen)
            return;

        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            Context.CameraShake.gameObject.SetActive(false);
            OnGamePaused?.Invoke();
        } else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
        }
    }


    
}
