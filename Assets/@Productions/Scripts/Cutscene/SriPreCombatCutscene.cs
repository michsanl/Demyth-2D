using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using MoreMountains.Feedbacks;
using Demyth.Gameplay;
using Core;

public class SriPreCombatCutscene : MonoBehaviour
{
    
    
    [SerializeField] private float _firstCutsceneStartDelay;
    [SerializeField] private float _secondCutsceneStartDelay;
    [SerializeField] private float _cameraMoveDownSequenceDuration;
    [Space]
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private SriCombatBehaviour _sriCombatBehaviour;

    private GameStateService _gameStateService;
    private Player _player;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            StartCoroutine(StartPreDialogueCutscene());
        }
    }

    public void OnConversationEnd()
    {
        StartCoroutine(StartPostDialogueCutscene());
    }

    private IEnumerator StartPreDialogueCutscene()
    {
        // SEQUENCE 1
        // disable player input
        // wait
        _gameStateService.SetState(GameState.Cutscene);
        yield return Helper.GetWaitForSeconds(_firstCutsceneStartDelay);

        // SEQUENCE 2
        // move camera up
        // wait
        yield return _cameraTransform.DOMoveY(9, 1f).SetEase(Ease.InOutCubic).WaitForCompletion();
        
        // SEQUENCE 3
        // initiate dialogue
        _dialogueSystemTrigger.OnUse();
    }

    private IEnumerator StartPostDialogueCutscene()
    {
        // SEQUENCE 4
        // wait
        yield return Helper.GetWaitForSeconds(_secondCutsceneStartDelay);

        // SEQUENCE 5
        // move camera down
        // wait
        _cameraTransform.DOMoveY(0, 1f).SetEase(Ease.InOutQuad);
        yield return Helper.GetWaitForSeconds(_cameraMoveDownSequenceDuration);

        // SEQUENCE 6
        // enable boss combat mode
        // enable player input
        // disable cutscene object
        _sriCombatBehaviour.InitiateCombatMode();
        _gameStateService.SetState(GameState.Gameplay);
        gameObject.SetActive(false);
    }

}
