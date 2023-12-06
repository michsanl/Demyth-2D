using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PetraPostCombatCutscene : MonoBehaviour
{
    
    [SerializeField] private PetraCombatBehaviour _petraCombatBehaviour;
    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;

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

    public void OnConversationEnd(Transform actor)
    {
        StartPostDialogueCutscene();
    }

    private IEnumerator StartPreDialogueCutscene()
    {
        // Disable player input
        _gameStateService.SetState(GameState.Cutscene);

        // Wait for petra on death animation to complete
        yield return Helper.GetWaitForSeconds(2.5f);

        // Start dialogue 
        _dialogueSystemTrigger.OnUse();
    }
    
    private void StartPostDialogueCutscene()
    {
        // Play petra reverse on death animation after dialogue
        _petraCombatBehaviour.PlayReviveAnimation();

        // Enable player input
        _gameStateService.SetState(GameState.Gameplay);
    }

}
