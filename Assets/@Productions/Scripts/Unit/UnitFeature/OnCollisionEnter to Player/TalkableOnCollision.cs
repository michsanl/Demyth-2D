using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TalkableOnCollision : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            dialogueSystemTrigger.OnUse();
        }
    }
}
