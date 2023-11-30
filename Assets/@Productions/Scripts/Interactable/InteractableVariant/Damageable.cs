using UnityEngine;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(Health))]
public class Damageable : Interactable
{
    private Health health;
    private MMF_Player panHitMMFPlayer;

    private void Awake()
    {
        health = GetComponent<Health>();
        panHitMMFPlayer = GetComponent<MMF_Player>();
    }

    public override void Interact(Player player, Vector3 dir = default)
    {
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
