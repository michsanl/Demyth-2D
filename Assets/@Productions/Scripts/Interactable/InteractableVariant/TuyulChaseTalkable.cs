using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

public class TuyulChaseTalkable : Interactable
{
    
    public Action<GameObject> OnAllTuyulHasBeenCaught;

    [SerializeField] private DialogueSystemTrigger _dialogueSystemTrigger;

    public override void Interact(Player player, Vector3 direction)
    {
        _dialogueSystemTrigger.OnUse();   
    }

    public void OnConversationEnd()
    {
        if (HasCaughtAllTuyul())
        {
            OnAllTuyulHasBeenCaught?.Invoke(this.gameObject);
        }
        gameObject.SetActive(false);
    }

    private bool HasCaughtAllTuyul()
    {
        return DialogueLua.GetVariable("Catch_Yula").AsBool && DialogueLua.GetVariable("Catch_Yuli").AsBool;
    }

}
