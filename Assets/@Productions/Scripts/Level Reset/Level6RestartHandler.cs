using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Core;
using System;
using Demyth.Gameplay;
using UISystem;

public class Level6RestartHandler : MonoBehaviour
{
    
    public Action OnLevelRestartEnabled;
    public Action OnLevelRestartDisabled;

    [SerializeField] private BoxPositionSO _boxPositionSO;
    [SerializeField] private Vector3 _playerResetPosition;
    [SerializeField] private Transform[] _boxArray;
    [SerializeField] private GameObject[] _hiddenItems;

    private GameStateService _gameStateService;
    private LoadingUI _loadingUI;
    private GameInput _gameInput;
    private GameInputController _inputController;
    private Player _player;
    private Transform _playerModel;
    private bool _isRestarting;
    private Vector3[] _boxInitialPositionArray;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _loadingUI = SceneServiceProvider.GetService<LoadingUI>();
        _inputController = SceneServiceProvider.GetService<GameInputController>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerModel = _player.PlayerModel;
        _gameInput = _inputController.GameInput;

        _boxInitialPositionArray = new Vector3[_boxArray.Length];
    }

    private void OnEnable()
    {
        if (IsLevelCompleted())
            return;

        _gameInput.OnRestartPerformed.AddListener(GameInput_OnRestartPerformed);
        SetBoxInitialPosition();

        OnLevelRestartEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        _gameInput.OnRestartPerformed.RemoveListener(GameInput_OnRestartPerformed);

        OnLevelRestartDisabled?.Invoke();
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
        _loadingUI.OpenPage();
        
        yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());

        ResetPlayer();
        ResetBoxPosition();
        ResetHiddenItem();
        _inputController.EnablePlayerInput();
        _loadingUI.ClosePage();

        yield return Helper.GetWaitForSeconds(_loadingUI.GetClosePageDuration());

        _inputController.EnablePauseInput();
        _isRestarting = false;
    }

    private void ResetPlayer()
    {
        _playerModel.localScale = Vector3.one;
        _player.transform.position = _playerResetPosition;
    }

    private void ResetHiddenItem()
    {
        foreach (var hiddenItem in _hiddenItems)
        {
            hiddenItem.SetActive(true);
        }
    }

    private void SetBoxInitialPosition()
    {
        for (int i = 0; i < _boxArray.Length; i++)
        {
            _boxInitialPositionArray[i] = _boxArray[i].position;
        }
    }

    private void ResetBoxPosition()
    {
        for (int i = 0; i < _boxArray.Length; i++)
        {
            _boxArray[i].transform.position = _boxInitialPositionArray[i];
        }
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable("Level_6_Done").AsBool;
    }
}
