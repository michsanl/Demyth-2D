using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;

public class Talkable : Interactable
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;


    public override void Interact()
    {
        dialogueSystemTrigger.OnUse();
        Debug.Log("Interact with NPC");
    }

    // public override void Push(Vector3 direction, float moveDuration)
    // {
    //     Vector3 finalDirection = transform.position + direction;
    //     transform.DOMove(finalDirection, moveDuration).SetEase(Ease.OutExpo);
    // }
}
