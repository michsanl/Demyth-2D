using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Core;
using System;
using PixelCrushers;
using Demyth.Gameplay;
using DG.Tweening;

public class TuyulChaseLevelReset : MonoBehaviour
{
    
    
    public Action OnTuyulLevelResetEnabled;
    public Action OnTuyulLevelResetDisabled;

    [SerializeField] private TuyulFleeMovement _yula;
    [SerializeField] private TuyulFleeMovement _yuli;
    [SerializeField] private GameObject _destructibleBox;
    [SerializeField] private Vector3 _playerResetPosition;
    [SerializeField] private Vector3 _yulaResetPosition;
    [SerializeField] private Vector3 _yuliResetPosition;
    [SerializeField] private Transform[] _BoxesArray;
    [SerializeField] private Vector3[] BoxesResetPositionArray;
    
    private GameInput _gameInput;
    private Player _player;

    private void Awake()
    {
        _gameInput = SceneServiceProvider.GetService<GameInputController>().GameInput;
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    private void OnEnable()
    {
        if (IsLevelCompleted())
            return;

        _gameInput.OnRestartPerformed.AddListener(GameInput_OnRestartPerformed);

        OnTuyulLevelResetEnabled?.Invoke();
    }

    private void OnDisable() 
    {
        _gameInput.OnRestartPerformed.RemoveListener(GameInput_OnRestartPerformed);

        OnTuyulLevelResetDisabled?.Invoke();
    }

    private void GameInput_OnRestartPerformed()
    {
        ResetTuyulLevel();
    }

    private void ResetTuyulLevel()
    {
        // reset player position
        // Reset tuyul active state, position, and visual
        // reset box position
        // restore destructible box
        DOTween.CompleteAll();

        _player.transform.position = _playerResetPosition;

        _yula.gameObject.SetActive(true);
        _yuli.gameObject.SetActive(true);
        _yula.transform.position = _yulaResetPosition;
        _yuli.transform.position = _yuliResetPosition;
        _yula.ResetUnitCondition();
        _yuli.ResetUnitCondition();

        for (int i = 0; i < _BoxesArray.Length; i++)
        {
            _BoxesArray[i].position = BoxesResetPositionArray[i];
        }

        _destructibleBox.SetActive(true);
        _destructibleBox.GetComponent<Health>().ResetHealthToMaximum();

        bool caughtState = false;
        DialogueLua.SetVariable("Catch_Yula", caughtState);
        DialogueLua.SetVariable("Catch_Yuli", caughtState);
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable("Catch_Yula").AsBool && DialogueLua.GetVariable("Catch_Yuli").AsBool;
    }
}
