using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PillarLight : Interactable
{
    public bool IsLightActive => isLightActive;
    public Action OnTurnOnLight;

    [SerializeField] private int hitToActivate = 6;
    [SerializeField] private GameObject lightModel;

    private MMF_Player panHitMMFPlayer;
    private int hitCount;
    private bool isLightActive = true;

    private void Awake()
    {
        panHitMMFPlayer = GetComponent<MMF_Player>();
    }

    public override void Interact(Player player, Vector3 direction = default)
    {
        panHitMMFeedback();

        if (!lightModel.activeSelf)
        {
            hitCount++;
            if (hitCount >= hitToActivate)
            {
                lightModel.SetActive(true);
                isLightActive = true;
                hitCount = 0;

                OnTurnOnLight?.Invoke();
            }
        }
        else
        {
            // lightModel.SetActive(false);
        }
    }

    public void TurnOnPillarLight()
    {
        lightModel.SetActive(true);
        isLightActive = true;
        hitCount = 0;
    }

    public void TurnOffPillarLight()
    {
        lightModel.SetActive(false);
        isLightActive = false;
        hitCount = 0;
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
