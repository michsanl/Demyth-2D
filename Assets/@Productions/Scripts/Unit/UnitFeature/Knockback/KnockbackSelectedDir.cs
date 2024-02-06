using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackSelectedDir : KnockbackBase
{
    
    [Space]
    [SerializeField] private KnockbackDirection knockbackDir;
    [SerializeField] private bool isCheckBlockedDir;

    private enum KnockbackDirection { Up, Down, Left, Right }

    public override Vector2 GetKnockbackTargetPosition(Player player)
    {
        this.player = player;
        knockbackOrigin = GetKnockBackOrigin();
        knockbackDirection = GetSelectedKnockBackDir();
        var knockbackTargetPosition = knockbackOrigin + knockbackDirection;
        var knockbackTargetPositionRounded = GetRoundedVectorValue(knockbackTargetPosition);

        if (isCheckBlockedDir && IsDirectionBlocked(knockbackDirection))
        {
            return knockbackOrigin;
        }

        return knockbackTargetPositionRounded;
    }

    private Vector2 GetSelectedKnockBackDir()
    {
        switch (knockbackDir)
        {
            case KnockbackDirection.Up:
                return Vector2.up;
            case KnockbackDirection.Down:
                return Vector2.down;
            case KnockbackDirection.Left:
                return Vector2.left;
            case KnockbackDirection.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}
