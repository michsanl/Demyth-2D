using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;

public class Talkable : Interactable
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;


    public override void Interact(Vector3 direction)
    {
        dialogueSystemTrigger.OnUse();
        Debug.Log("Interact with NPC");
    }
}
