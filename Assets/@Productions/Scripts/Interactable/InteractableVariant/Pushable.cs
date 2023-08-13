using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Pushable : Interactable
{
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private int raycastOriginOffsetX;
    [SerializeField] private int raycastOriginOffsetY;

    public static Action OnAnyPushableInteract;

    private BoxCollider2D boxCollider;

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Interact(Player player, Vector3 direction)
    {
        OnAnyPushableInteract?.Invoke();
        var raycastOrigin = transform.position + GetRaycastOriginOffset(direction);
        if (Helper.CheckTargetDirection(raycastOrigin, direction, boxCollider.size, movementBlockerLayerMask, out Interactable interactable))
            return;
            
        Move(direction);

    }

    private void Move(Vector3 direction)
    {
        var moveTargetLocation = transform.position + direction;
        Helper.MoveToPosition(transform, moveTargetLocation, 0.2f);
    }

    private Vector3 GetRaycastOriginOffset(Vector3 direction)
    {
        direction.x = direction.x * raycastOriginOffsetX;
        direction.y = direction.y * raycastOriginOffsetY;
        return direction;
    }
}
