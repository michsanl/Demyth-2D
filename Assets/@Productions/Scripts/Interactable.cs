using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// diinherit sama class Pushable, Talkable, sama LevelChanger
public class Interactable : MonoBehaviour
{
    public InteractableType interactableType;
    
    public virtual void Talk() 
    {
    }
    public virtual void Push(Vector3 direction, float pushDuration) 
    {
    }
    public virtual void ChangeLevel()
    {
    }

    public virtual void Interact(Vector3 direction)
    {
    }
}

public enum InteractableType
{
    Talkable, Pushable, LevelChanger,
}
