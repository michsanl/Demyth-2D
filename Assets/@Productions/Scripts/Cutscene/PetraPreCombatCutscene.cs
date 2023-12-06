using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using MoreMountains.Feedbacks;
using Demyth.Gameplay;
using Core;

public class PetraPreCombatCutscene : MonoBehaviour
{
    
    [SerializeField] private float _firstCutsceneStartDelay;
    [SerializeField] private float _cameraMoveUpDuration;
    [SerializeField] private float _secondCutsceneStartDelay;
    [Space]
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _petraCombatGameObject;
    [SerializeField] private GameObject _petraIdleGameObject;

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
        _gameStateService.SetState(GameState.Cutscene);
        // TIMELINE 1
        yield return new WaitForSeconds(_firstCutsceneStartDelay);

        // TIMELINE 2
        // move camera up
        _cameraTransform.DOMoveY(10, 1f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(_cameraMoveUpDuration);
        
        // TIMELINE 3
        // initiate dialogue
        _dialogueSystemTrigger.OnUse();
    }

    private IEnumerator StartPostDialogueCutscene()
    {
        // TIMELINE 4
        yield return new WaitForSeconds(_secondCutsceneStartDelay);

        // TIMELINE 5
        // disable petra idle
        // enable petra combat
        // move camera down
        // disable cutscene
        _petraIdleGameObject.SetActive(false);
        _petraCombatGameObject.SetActive(true);
        _cameraTransform.DOMoveY(0, 1f).SetEase(Ease.InOutQuad);
        _gameStateService.SetState(GameState.Gameplay);
        _player.UsePan = true;
        gameObject.SetActive(false);
    }
}
