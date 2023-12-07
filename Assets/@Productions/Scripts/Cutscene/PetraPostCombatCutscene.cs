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
    
    [SerializeField] private PetraCombatBehaviour _petraCombatBehaviour;
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;
    [SerializeField] private GameObject _unlockableInvisibleWall;

    private GameStateService _gameStateService;
    private Health _petraHealth;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _petraHealth = _petraCombatBehaviour.GetComponent<Health>();

        _petraHealth.OnDeath += PetraHealth_OnDeath;
    }

    private void PetraHealth_OnDeath()
    {
        StartCoroutine(StartPreDialogueCutscene());
    }

    public void OnConversationEnd()
    {
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
        _gameStateService.SetState(GameState.Cutscene);
        _dialogueSystemTrigger.OnUse();
    }
    
    private void StartPostDialogueCutscene()
    {
        // SEQUENCE 3
        // Enable player input
        // Play petra reverse on death animation after dialogue
        // disable blocking wall
        // save
        _gameStateService.SetState(GameState.Gameplay);
        _petraCombatBehaviour.PlayReviveAnimation();
        _unlockableInvisibleWall.SetActive(false);
        SaveSystem.SaveToSlot(1);
    }

}
