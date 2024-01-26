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

public class Level5RestartHandler : MonoBehaviour
{
    
    public Action OnTuyulLevelResetEnabled;
    public Action OnTuyulLevelResetDisabled;

    [SerializeField] private BoxPositionSO _boxPositionSO;
    [SerializeField] private TuyulFleeMovement _yula;
    [SerializeField] private TuyulFleeMovement _yuli;
    [SerializeField] private Vector3 _playerResetPosition;
    [SerializeField] private Transform[] _boxArray;

    private LoadingUI _loadingUI;
    private GameInput _gameInput;
    private GameInputController _inputController;
    private Player _player;
    private Transform _playerModel;

    private void Awake()
    {
        _loadingUI = SceneServiceProvider.GetService<LoadingUI>();
        _inputController = SceneServiceProvider.GetService<GameInputController>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerModel = _player.PlayerModel;
        _gameInput = _inputController.GameInput;
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
        _inputController.DisableRestartInput();
        _inputController.DisablePlayerInput();
        
        StartCoroutine(ResetLevel());
    }

    private IEnumerator ResetLevel()
    {

        _loadingUI.OpenPage();
        yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());

        DOTween.CompleteAll();
        ResetPlayer();
        ResetTuyul();
        ResetBox();
        _inputController.EnablePlayerInput();

        _loadingUI.ClosePage();
        yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());

        _inputController.EnableRestartInput();
    }

    private void ResetPlayer()
    {
        _playerModel.localScale = Vector3.one;
        _player.transform.position = _playerResetPosition;
    }

    private void ResetTuyul()
    {
        _yula.gameObject.SetActive(true);
        _yuli.gameObject.SetActive(true);
        _yula.transform.position = _boxPositionSO.YulaPosition;
        _yuli.transform.position = _boxPositionSO.YuliPosition;
        _yula.ResetUnitCondition();
        _yuli.ResetUnitCondition();
        DialogueLua.SetVariable("Catch_Yula", false);
        DialogueLua.SetVariable("Catch_Yuli", false);
    }

    private void ResetBox()
    {
        for (int i = 0; i < _boxArray.Length; i++)
        {
            _boxArray[i].transform.position = _boxPositionSO.BoxWoodPositionArray[i];
        }
    }

    private bool IsLevelCompleted()
    {
        return DialogueLua.GetVariable("Catch_Yula").AsBool && DialogueLua.GetVariable("Catch_Yuli").AsBool;
    }



    private void OldResetTuyulLevel()
    {
        // reset player position
        // Reset tuyul active state, position, and visual
        // reset box position
        // restore destructible box
        // DOTween.CompleteAll();

        // _playerModel.localScale = Vector3.one;
        // _player.transform.position = _playerResetPosition;

        // _yula.gameObject.SetActive(true);
        // _yuli.gameObject.SetActive(true);
        // _yula.transform.position = _yulaResetPosition;
        // _yuli.transform.position = _yuliResetPosition;
        // _yula.ResetUnitCondition();
        // _yuli.ResetUnitCondition();

        // for (int i = 0; i < _boxArray.Length; i++)
        // {
        //     _boxArray[i].position = BoxesResetPositionArray[i];
        // }

        // _destructibleBox.SetActive(true);
        // _destructibleBox.GetComponent<Health>().ResetHealthToMaximum();

        // bool caughtState = false;
        // DialogueLua.SetVariable("Catch_Yula", caughtState);
        // DialogueLua.SetVariable("Catch_Yuli", caughtState);
    }
}
