using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Core;
using System;
using Demyth.Gameplay;
using UISystem;
using UnityEngine.Rendering.Universal;

public class Level6RestartHandler : SceneService
{
    
    public Action OnRestartHandlerEnabled;
    public Action OnRestartHandlerDisabled;

    [Space]
    [SerializeField] private Level6PuzzlePositionSO _level6PositionSO;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private GameObject[] _gates;
    [SerializeField] private Transform[] _boxArray;
    [SerializeField] private GameObject[] _hiddenItems;

    private GameStateService _gameStateService;
    private GameInput _gameInput;
    private GameInputController _inputController;
    private Player _player;
    private Transform _playerModel;
    private bool _isRestarting;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _inputController = SceneServiceProvider.GetService<GameInputController>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerModel = _player.PlayerModel;
        _gameInput = _inputController.GameInput;
    }

    private void OnEnable()
    {
        _gameInput.OnRestartPerformed.AddListener(GameInput_OnRestartPerformed);

        OnRestartHandlerEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        _gameInput.OnRestartPerformed.RemoveListener(GameInput_OnRestartPerformed);
        
        _inputController.EnablePauseInput();
        _isRestarting = false;

        OnRestartHandlerDisabled?.Invoke();
    }

    private void GameInput_OnRestartPerformed()
    {
        if (_isRestarting) return;
        if (_gameStateService.CurrentState == GameState.Pause) return;
        
        _inputController.DisablePauseInput();
        _inputController.DisablePlayerInput();
        
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        _isRestarting = true;
        
        yield return StartCoroutine(PersistenceLoadingUI.Instance.OpenLoadingPage());

        ResetPlayer();
        ResetBoxPosition();
        ResetHiddenItem();
        ResetQuest();
        ResetLight();
        DeactivateGate();
        _inputController.EnablePlayerInput();

        yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());

        _inputController.EnablePauseInput();
        _isRestarting = false;
    }

    private void ResetPlayer()
    {
        _playerModel.localScale = new Vector3(-1, 1, 1);
        _player.transform.position = _level6PositionSO.PlayerPosition;
        _player.IsShieldUnlocked = false;
    }

    private void ResetHiddenItem()
    {
        foreach (var hiddenItem in _hiddenItems)
        {
            hiddenItem.SetActive(true);
        }

        int collectedPaperCount = 0;
        DialogueLua.SetVariable("HiddenItem.NumCollected", collectedPaperCount);
    }

    private void ResetBoxPosition()
    {
        for (int i = 0; i < _boxArray.Length; i++)
        {
            _boxArray[i].transform.position = _level6PositionSO.BoxPositions[i];
        }
    }

    private void ResetQuest()
    {
        QuestLog.SetQuestState("Sparkling Hidden Item", QuestState.Active);
        DialogueLua.SetVariable("Level_6_Puzzle_Done", false);
    }

    private void ResetLight()
    {
        _globalLight.intensity = 0f;
    }

    private void DeactivateGate()
    {
        foreach (var gate in _gates)
        {
            gate.SetActive(false);
        }
    }
}
