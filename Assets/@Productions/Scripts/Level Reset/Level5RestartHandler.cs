using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Core;
using System;
using PixelCrushers;
using Demyth.Gameplay;
using DG.Tweening;
using UISystem;

public class Level5RestartHandler : SceneService
{
    
    public Action OnRestartHandlerEnabled;
    public Action OnRestartHandlerDisabled;

    [Space]
    [SerializeField] private Level5PuzzlePositionSO _level5PuzzlePositionSO;
    [SerializeField] private TuyulFleeMovement _yula;
    [SerializeField] private TuyulFleeMovement _yuli;
    [SerializeField] private Transform[] _boxArray;

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
        ResetTuyul();
        ResetBox();
        _inputController.EnablePlayerInput();

        yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());

        _inputController.EnablePauseInput();
        _isRestarting = false;
    }

    private void ResetPlayer()
    {
        _playerModel.localScale = Vector3.one;
        _player.transform.position = _level5PuzzlePositionSO.PlayerPosition;
    }

    private void ResetTuyul()
    {
        _yula.gameObject.SetActive(true);
        _yuli.gameObject.SetActive(true);
        _yula.transform.position = _level5PuzzlePositionSO.YulaPosition;
        _yuli.transform.position = _level5PuzzlePositionSO.YuliPosition;
        _yula.ResetUnitCondition();
        _yuli.ResetUnitCondition();
        DialogueLua.SetVariable("Catch_Yula", false);
        DialogueLua.SetVariable("Catch_Yuli", false);
    }

    private void ResetBox()
    {
        for (int i = 0; i < _boxArray.Length; i++)
        {
            _boxArray[i].transform.position = _level5PuzzlePositionSO.BoxPositions[i];
        }
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable("Catch_Yula").AsBool && DialogueLua.GetVariable("Catch_Yuli").AsBool;
    }
}
