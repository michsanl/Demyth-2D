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
    [SerializeField] private SriCombatBehaviour _sriCombatBehaviour;

    private CameraController _cameraController;
    private GameInputController _gameInputController;

    private void Awake()
    {
        _cameraController = SceneServiceProvider.GetService<CameraController>();
        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
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
        _gameInputController.DisablePlayerInput();
        yield return Helper.GetWaitForSeconds(_firstCutsceneStartDelay);

        // SEQUENCE 2
        // move camera up
        // wait
        _cameraController.DOMoveYCamera(9f, 1f, Ease.InOutCubic);
        yield return new WaitForSeconds(1f);
        
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
        _cameraController.DOMoveYCamera(0f, 1f, Ease.InOutQuad);
        yield return Helper.GetWaitForSeconds(_cameraMoveDownSequenceDuration);

        // SEQUENCE 6
        // enable boss combat mode
        // enable player input
        // disable cutscene object
        _sriCombatBehaviour.InitiateCombat();
        _gameInputController.EnablePlayerInput();
        gameObject.SetActive(false);
    }

}
