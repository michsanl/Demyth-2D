using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// diinherit sama class Pushable, Talkable, sama LevelChanger
public class Interactable : MonoBehaviour
{
    
    public virtual void Interact(Vector3 direction = default(Vector3))
    {
    }
    
}
