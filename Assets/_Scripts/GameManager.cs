using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public FirstLevelState State;

    [SerializeField] private Transform mainMenuUI;
    [SerializeField] private Transform loadingUI;
    
    private void Start()
    {
        loadingUI.gameObject.SetActive(false);
        SetState(FirstLevelState.Play);
    }

    public void SetState(FirstLevelState newState) {
        
        State = newState;
        switch (State)
        {
            case FirstLevelState.MainMenu:
                HandleMainMenu();
                break;
            case FirstLevelState.Play:
                HandlePlay();
                break;
            case FirstLevelState.Pause:
                break;
            case FirstLevelState.ExitLevel:
                HandleExitLevel();
                break;
            default:
                break;
        }
    }


    private void HandleMainMenu()
    {
        mainMenuUI.gameObject.SetActive(true);
    }
    private void HandlePlay()
    {
        mainMenuUI.gameObject.SetActive(false);
    }
    private void HandleExitLevel()
    {
        loadingUI.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void PlayButton()
    {
        SetState(FirstLevelState.Play);
    }
    
}

public enum FirstLevelState {
    MainMenu,
    Pause,
    Play,
    ExitLevel,
}
