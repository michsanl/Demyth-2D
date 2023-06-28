using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using UnityEngine.UI;
using System;
using PixelCrushers.DialogueSystem;

public class HUDUI : SceneService
{
    [SerializeField] private Image healthPointImage;
    [SerializeField] private Image healthPotionEmptyImage;
    [SerializeField] private Image senterLightOnImage;

    private Animator animator;
    private HealthPotion playerHealthPotion;
    private bool isOpen;

    protected override void OnInitialize()
    {
        animator = GetComponent<Animator>();

        Open();
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        
        playerHealthPotion = Context.Player.GetComponent<HealthPotion>();

        playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
        Context.Player.OnSenterToggle += Player_OnSenterToggle;
        DialogueManager.instance.conversationStarted += DialogueManager_ConversationStarted;
        DialogueManager.instance.conversationEnded += DialogueManager_ConversationEnded;
    }

    private void PlayerHealthPotion_OnUsePotion(int healthPotionAmount)
    {
        if (healthPotionAmount == 0)
        {
            healthPotionEmptyImage.gameObject.SetActive(true);
        }
        else
        {
            healthPotionEmptyImage.gameObject.SetActive(false);
        }
    }

    private void Player_OnSenterToggle(bool senterState)
    {
        senterLightOnImage.gameObject.SetActive(senterState);
    }

    private void DialogueManager_ConversationStarted(Transform t)
    {
        Close();
    }

    private void DialogueManager_ConversationEnded(Transform t)
    {
        Open();
    }

    public void Open()
    {
        if (isOpen)
            return;

        animator.Play("HUD_Open");
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        animator.Play("HUD_Close");
        isOpen = false;
    }
}
