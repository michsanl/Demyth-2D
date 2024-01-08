using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demyth.Gameplay;
using Core;
using System;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class Level3RestartHandler : MonoBehaviour
{
    
    public Action OnBoxPuzzleLevelResetEnabled;
    public Action OnBoxPuzzleLevelResetDisabled;

    [SerializeField] private Transform _playerModel;
    [SerializeField] private Transform[] _boxCrateArray;
    [SerializeField] private Transform[] _boxCardBoardOpenArray;
    [SerializeField] private Transform[] _boxCardboardClosedArray;
    [SerializeField] private BoxPuzzleResetPositionSO _resetPositionSO;
    
    private Player _player;
    private GameInput _gameInput;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _gameInput = SceneServiceProvider.GetService<GameInputController>().GameInput;
    }

    private void OnEnable()
    {
        if (IsLevelCompleted())
            return;

        _gameInput.OnRestartPerformed.AddListener(GameInput_OnRestartPerformed);

        OnBoxPuzzleLevelResetEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        _gameInput.OnRestartPerformed.RemoveListener(GameInput_OnRestartPerformed);

        OnBoxPuzzleLevelResetDisabled?.Invoke();
    }

    private void GameInput_OnRestartPerformed()
    {
        ResetBoxPuzzleLevel();
    }

    private void ResetBoxPuzzleLevel()
    {
        // complete all tween
        // reset player position & animation
        // reset box position
        DOTween.CompleteAll();

        _playerModel.localScale = Vector3.one;
        _player.SetAnimationToIdleNoPan();
        ResetPlayerPosition();
        ResetBoxCratePosition();
        ResetBoxCardboardOpenPosition();
        ResetBoxCardboardClosedPosition();
    }

    private void ResetPlayerPosition()
    {
        _player.transform.position = _resetPositionSO.PlayerResetPosition;
    }

    private void ResetBoxCratePosition()
    {
        for (int i = 0; i < _boxCrateArray.Length; i++)
        {
            _boxCrateArray[i].position = _resetPositionSO.BoxCrateResetPositionArray[i];
        }
    }

    private void ResetBoxCardboardOpenPosition()
    {
        for (int i = 0; i < _boxCardBoardOpenArray.Length; i++)
        {
            _boxCardBoardOpenArray[i].position = _resetPositionSO.BoxCardBoardOpenResetPositionArray[i];
        }
    }

    private void ResetBoxCardboardClosedPosition()
    {
        for (int i = 0; i < _boxCardboardClosedArray.Length; i++)
        {
            _boxCardboardClosedArray[i].position = _resetPositionSO.BoxCardboardClosedResetPositionArray[i];
        }
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable("Level_3_Done").asBool;
    }
}
