using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Pickupable : Interactable
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;
    [SerializeField] private GameObject mainGameObject;

    public override void Interact(Vector3 direction = default)
    {
        dialogueSystemTrigger.OnUse();
    }
    
}
