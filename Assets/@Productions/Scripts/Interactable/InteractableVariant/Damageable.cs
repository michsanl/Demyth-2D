using UnityEngine;
using MoreMountains.Feedbacks;
using Core;
using Demyth.Gameplay;

[RequireComponent(typeof(Health))]
public class Damageable : Interactable
{
    private GameStateService _gameStateService;
    private Health health;
    private MMF_Player panHitMMFPlayer;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        health = GetComponent<Health>();
        panHitMMFPlayer = GetComponent<MMF_Player>();
    }

    public override void Interact(Player player, Vector3 dir = default)
    {
        if (_gameStateService.CurrentState == GameState.GameOver) return;

        panHitMMFeedback();

        health.TakeDamage();
    }

    private void panHitMMFeedback()
    {
        // Pan hit sound
        // Player hit effect

        MMF_InstantiateObject instantiateMMFPlayer = panHitMMFPlayer.GetFeedbackOfType<MMF_InstantiateObject>();
        instantiateMMFPlayer.TargetTransform = transform;
        
        panHitMMFPlayer.PlayFeedbacks();
    }
}
