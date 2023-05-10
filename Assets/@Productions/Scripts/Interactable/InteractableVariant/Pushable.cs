using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pushable : Interactable
{
    [SerializeField]
    private LayerMask movementBlockerLayerMask;

    public override void Interact(Vector3 direction)
    {
        Vector3 targetLocation = transform.position + direction;

        if (Helper.CheckTargetDirection(transform.position, direction, movementBlockerLayerMask, out Interactable interactable)) return;

        Helper.MoveToPosition(transform, targetLocation, 0.2f);        
    }
}
