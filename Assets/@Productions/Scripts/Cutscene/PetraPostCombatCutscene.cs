using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Core;
using Demyth.Gameplay;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PetraPostCombatCutscene : MonoBehaviour
{
    
    [SerializeField] private GameObject _bossPetraGameObject;
    [SerializeField] private GameObject _npcPetraGameObject;
    [SerializeField] private Transform _bossPetraModel;
    [SerializeField] private Transform _npcPetraModel;
    [SerializeField] private Animator _npcPetraAnimator;
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
    [SerializeField] private GameObject _unlockableInvisibleWall;

    private GameStateService _gameStateService;
    private GameInputController _gameInputController;
    private Health _petraHealth;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameInputController = SceneServiceProvider.GetService<GameInputController>();
        _petraHealth = _bossPetraGameObject.GetComponent<Health>();

        _petraHealth.OnDeath += PetraHealth_OnDeath;
    }

    private void PetraHealth_OnDeath()
    {
        StartCoroutine(StartPreDialogueCutscene());
    }

    public void OnConversationEnd()
    {
        if (_gameStateService.CurrentState == GameState.MainMenu) return;
        
        StartPostDialogueCutscene();
    }

    private IEnumerator StartPreDialogueCutscene()
    {
        // SEQUENCE 1
        // Wait for petra on death animation to complete
        yield return Helper.GetWaitForSeconds(2.5f);

        // SEQUENCE 2
        // Disable player input
        // Start dialogue 
        _gameInputController.DisablePlayerInput();
        _dialogueSystemTrigger.OnUse();
    }
    
    private void StartPostDialogueCutscene()
    {
        // SEQUENCE 3
        // Enable player input
        // Disable boss petraa
        // Enable non-boss petra
        // Set position and facing direction on non-boss petra
        // play revive animation on non-boss petra
        // disable blocking wall
        // save
        _gameInputController.EnablePlayerInput();

        _bossPetraGameObject.SetActive(false);
        _npcPetraGameObject.SetActive(true);

        _npcPetraGameObject.transform.position = _bossPetraGameObject.transform.position;
        _npcPetraModel.localScale = _bossPetraModel.localScale;
        _npcPetraAnimator.Play("Revive");

        _unlockableInvisibleWall.SetActive(false);
        SaveSystem.SaveToSlot(1);
    }

}
