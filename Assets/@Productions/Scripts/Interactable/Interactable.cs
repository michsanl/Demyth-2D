using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// diinherit sama class Pushable, Talkable, sama LevelChanger
public class Interactable : MonoBehaviour
{
    public InteractableType interactableType;
    
    public virtual void Interact(Vector3 direction = default(Vector3))
    {
    }
}

public enum InteractableType
{
    Talk, Push, ChangeLevel, Damage,
}
