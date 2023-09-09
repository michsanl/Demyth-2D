using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using UnityEngine;

// diinherit sama class Pushable, Talkable, sama LevelChanger
public class Interactable : SceneService
{
    
    public virtual void Interact(Player player, Vector3 direction = default(Vector3))
    {
    }
    
}
