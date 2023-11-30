using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PillarLight : Interactable
{
    [SerializeField] private int hitToActivate = 6;
    [SerializeField] private GameObject lightModel;

    private MMF_Player panHitMMFPlayer;
    private int hitCount;

    private void Awake()
    {
        panHitMMFPlayer = GetComponent<MMF_Player>();
    }

    public override void Interact(Player player, Vector3 direction = default)
    {
        panHitMMFeedback();

        if (!lightModel.activeInHierarchy)
        {
            hitCount++;
            if (hitCount >= hitToActivate)
            {
                lightModel.SetActive(true);
                hitCount = 0;
            }
        }
        else
        {
            lightModel.SetActive(false);
        }
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
