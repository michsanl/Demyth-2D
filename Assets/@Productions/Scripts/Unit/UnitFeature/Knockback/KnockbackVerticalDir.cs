using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackVerticalDir : KnockbackBase
{
    public override Vector2 GetKnockbackTargetPosition(Player player)
    {
        this.player = player;
        knockbackOrigin = GetKnockBackOrigin();
        finalKnockbackDir = GetVerticalKnockBackDir();

        return GetFinalKnockbackTargetPosition(finalKnockbackDir);
    }

    private Vector2 GetVerticalKnockBackDir()
    {
        Vector2 knockBackDir = GetInitialVerticalKnockBackDir();

        int loopCount = 2;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir))
                return knockBackDir;

            knockBackDir = GetOppositeDirection(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector2 GetInitialVerticalKnockBackDir()
    {
        if (player.transform.position.y > transform.position.y)
            return Vector2.up;
        if (player.transform.position.y < transform.position.y)
            return Vector2.down;
        return UnityEngine.Random.Range(0, 2) == 0 ? Vector2.up : Vector2.down;
    }

    private Vector2 GetOppositeDirection(Vector2 dir)
    {
        dir.x = Mathf.RoundToInt(dir.x * -1f); 
        dir.y = Mathf.RoundToInt(dir.y * -1f);
        return dir;
    }
}
