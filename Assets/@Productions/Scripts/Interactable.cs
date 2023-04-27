using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// diinherit sama class Pushable, Talkable, sama LevelChanger
public class Interactable : MonoBehaviour
{
    public virtual void Talk() 
    {
    }
    public virtual void Push(Vector3 direction, float pushDuration) 
    {
    }
    public virtual void ChangeLevel()
    {
    }
}
