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
    [SerializeField] private Image healthPotionImage;
    [SerializeField] private Image senterLightOnImage;

    private Animator animator;
    private bool isOpen;

    protected override void OnInitialize()
    {
        animator = GetComponent<Animator>();

        Context.Player.OnSenterToggle += Player_OnSenterToggle;
        DialogueManager.instance.conversationStarted += DialogueManager_ConversationStarted;
        DialogueManager.instance.conversationEnded += DialogueManager_ConversationEnded;

        Open();
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
