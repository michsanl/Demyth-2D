using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using System;

public class TalkableOnCollision : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger dialogueSystemTrigger;

    public Action OnDeathSlashCollision;

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            OnDeathSlashCollision?.Invoke();
        }
    }
}
