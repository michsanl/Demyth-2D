using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MySingleton<GameManager>
{
    public GameState State;

    [SerializeField] private Transform mainMenuUI;
    [SerializeField] private Transform loadingUI;
    [SerializeField] private GameObject[] levelPrefabArray;
    
    private void Start()
    {
        loadingUI.gameObject.SetActive(false);
        SetState(GameState.Play);
    }

    public void SetState(GameState newState) 
    {
        
        State = newState;
        switch (State)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Pause:
                break;
            case GameState.ExitLevel:
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
        levelPrefabArray[0].gameObject.SetActive(false);
        loadingUI.gameObject.SetActive(true);
        levelPrefabArray[1].gameObject.SetActive(true);
        SetState(GameState.Play);
    }

    public void PlayButton()
    {
        SetState(GameState.Play);
    }
    
}

public enum GameState 
{
    MainMenu,
    Pause,
    Play,
    ExitLevel,
}
