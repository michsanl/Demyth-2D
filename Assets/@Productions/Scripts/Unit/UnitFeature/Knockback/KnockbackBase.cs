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
    protected Vector2 finalKnockbackDir;
    protected Vector2 knockbackOrigin;

    protected enum KnockbackSource { ThisObject, Player }

    public abstract Vector2 GetKnockbackTargetPosition(Player player);


    protected Vector2 GetKnockBackOrigin()
    {
        return knockbackSource == KnockbackSource.ThisObject ? transform.position : player.LastMoveTargetPosition;
    }
    
    protected Vector2 GetFinalKnockbackTargetPosition(Vector2 knockBackDir)
    {
        return new Vector2(Mathf.RoundToInt(knockBackDir.x + knockbackOrigin.x), Mathf.RoundToInt(knockBackDir.y + knockbackOrigin.y));
    }
    
    protected bool IsDirectionBlocked(Vector2 knockBackDir)
    {
        var moveBlockerAhead = Helper.CheckTargetDirection(knockbackOrigin, knockBackDir, moveBlockMask, out Interactable interactable);
        var damagingPlayerAhead = Physics.Raycast(knockbackOrigin, knockBackDir, 1f, damagePlayerMask); 

        return moveBlockerAhead || damagingPlayerAhead;
    }
}
