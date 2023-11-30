using UnityEngine;
using MoreMountains.Feedbacks;

public class Pushable : Interactable
{
    [SerializeField] private LayerMask movementBlockerLayerMask;
    [SerializeField] private int raycastOriginOffsetX;
    [SerializeField] private int raycastOriginOffsetY;

    private BoxCollider2D boxCollider;
    private MMF_Player boxHitMMFPlayer;

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxHitMMFPlayer = GetComponent<MMF_Player>();
    }

    public override void Interact(Player player, Vector3 direction)
    {
        BoxHitMMFeedback();

        if (IsMoveDirectionBlocked(direction))
            return;
            
        Move(direction);
    }

    private void Move(Vector3 direction)
    {
        var moveTargetLocation = transform.position + direction;
        Helper.MoveToPosition(transform, moveTargetLocation, 0.2f);
    }

    private void BoxHitMMFeedback()
    {
        // Box hit sound
        // Player hit effect

        MMF_InstantiateObject instantiateMMFPlayer = boxHitMMFPlayer.GetFeedbackOfType<MMF_InstantiateObject>();
        instantiateMMFPlayer.TargetTransform = transform;
        
        boxHitMMFPlayer.PlayFeedbacks();
    }

    private bool IsMoveDirectionBlocked(Vector3 direction)
    {
        var raycastOrigin = transform.position + GetRaycastOriginOffset(direction);
        if (Helper.CheckTargetDirection(raycastOrigin, direction, boxCollider.size, movementBlockerLayerMask, out Interactable interactable))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private Vector3 GetRaycastOriginOffset(Vector3 direction)
    {
        direction.x = direction.x * raycastOriginOffsetX;
        direction.y = direction.y * raycastOriginOffsetY;
        return direction;
    }
}
