using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHorizontalDir : KnockbackBase
{
    public override Vector2 GetKnockbackTargetPosition(Player player)
    {
        this.player = player;
        knockbackOrigin = GetKnockBackOrigin();
        finalKnockbackDir = GetHorizontalKnockBackDir();

        return GetFinalKnockbackTargetPosition(finalKnockbackDir);
    }

    private Vector2 GetHorizontalKnockBackDir()
    {
        Vector2 knockBackDir = GetInitialHorizontalKnockBackDir();

        int loopCount = 2;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir))
                return knockBackDir;

            knockBackDir = GetOppositeDirection(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector2 GetInitialHorizontalKnockBackDir()
    {
        if (player.transform.position.x > transform.position.x)
            return Vector2.right;
        if (player.transform.position.x < transform.position.x)
            return Vector2.left;
        return UnityEngine.Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
    }

    private Vector2 GetOppositeDirection(Vector2 dir)
    {
        dir.x = Mathf.RoundToInt(dir.x * -1f); 
        dir.y = Mathf.RoundToInt(dir.y * -1f);
        return dir;
    }
}
