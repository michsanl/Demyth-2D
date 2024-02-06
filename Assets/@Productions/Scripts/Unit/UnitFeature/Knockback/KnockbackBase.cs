using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using UnityEngine;

public abstract class KnockbackBase : MonoBehaviour
{
    [SerializeField] protected KnockbackSource knockbackSource;
    [Space]
    [SerializeField] protected LayerMask moveBlockMask;
    [SerializeField] protected LayerMask damagePlayerMask;

    protected Player player;
    protected Vector2 knockbackDirection;
    protected Vector2 knockbackOrigin;

    protected enum KnockbackSource { ThisObject, Player }

    public abstract Vector2 GetKnockbackTargetPosition(Player player);


    protected Vector2 GetKnockBackOrigin()
    {
        return knockbackSource == KnockbackSource.ThisObject ? transform.position : player.LastMoveTargetPosition;
    }

    protected Vector2 GetRoundedVectorValue(Vector2 targetVector)
    {
        targetVector.x = Mathf.RoundToInt(targetVector.x);
        targetVector.y = Mathf.RoundToInt(targetVector.y);
        return targetVector;
    }
    
    protected bool IsDirectionBlocked(Vector2 knockBackDir)
    {
        var moveBlockerAhead = Helper.CheckTargetDirection(knockbackOrigin, knockBackDir, moveBlockMask, out Interactable interactable);
        var damagingPlayerAhead = Physics.Raycast(knockbackOrigin, knockBackDir, 1f, damagePlayerMask); 

        return moveBlockerAhead || damagingPlayerAhead;
    }
}
