using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackCheckAllDir : KnockbackBase
{
    public override Vector2 GetKnockbackTargetPosition(Player player)
    {
        this.player = player;
        knockbackOrigin = GetKnockBackOrigin();
        finalKnockbackDir = GetAllKnockBackDir();

        return GetFinalKnockbackTargetPosition(finalKnockbackDir);
    }

    private Vector2 GetAllKnockBackDir()
    {
        Vector2 knockBackDir = GetInitialDirToCheck();

        if (knockBackDir == Vector2.zero)
            knockBackDir = player.PlayerDir;

        int loopCount = 4;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir))
                return knockBackDir;
            
            knockBackDir = Vector2.Perpendicular(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector3 GetInitialDirToCheck()
    {
        return (player.transform.position - transform.position).normalized;
    }
}
