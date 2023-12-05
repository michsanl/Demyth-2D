using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using MoreMountains.Feedbacks;

public class PetraCutscene : MonoBehaviour
{
    
    [SerializeField] private float _firstCutsceneStartDelay;
    [SerializeField] private float _cameraMoveUpDuration;
    [SerializeField] private float _secondCutsceneStartDelay;
    [Space]
    [SerializeField] private MMF_Player _mmfPlayerCameraMoveUp;
    [SerializeField] private MMF_Player _mmfPlayerCameraMoveDown;
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _petraCombatGameObject;
    [SerializeField] private GameObject _petraIdleGameObject;

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            StartCoroutine(StartFirstCutsceneSequence());
        }
    }

    public void StartPostDialogueCutscene()
    {
        StartCoroutine(StartSecondCutsceneSequence());
    }

    private IEnumerator StartFirstCutsceneSequence()
    {
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

    private IEnumerator StartSecondCutsceneSequence()
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
        gameObject.SetActive(false);
    }
}
