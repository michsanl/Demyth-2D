using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pushable : Interactable
{
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private Vector2 size;
    [SerializeField] private int castOriginOffsetX;
    [SerializeField] private int castOriginOffsetY;

    public override void Interact(Vector3 direction)
    {
        var targetLocation = transform.position + direction;

        var raycastOrigin = transform.position + GetRaycastOriginOffset(direction);

        if (Helper.CheckTargetDirection(raycastOrigin, direction, size, movementBlockerLayerMask, out Interactable interactable)) return;

        Helper.MoveToPosition(transform, targetLocation, 0.2f);        
    }

    private Vector3 GetRaycastOriginOffset(Vector3 direction)
    {
        direction.x = direction.x * castOriginOffsetX;
        direction.y = direction.y * castOriginOffsetY;
        return direction;
    }
}
