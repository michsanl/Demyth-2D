using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using System;

public class Talkable : Interactable
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;

    public static Action OnAnyTalkbleInteract;

    public override void Interact(Player player, Vector3 direction)
    {
        OnAnyTalkbleInteract?.Invoke();
        dialogueSystemTrigger.OnUse();
    }
}
