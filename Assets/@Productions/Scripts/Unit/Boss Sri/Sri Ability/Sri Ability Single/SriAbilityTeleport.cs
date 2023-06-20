using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SriAbilityTeleport : MonoBehaviour
{
    public void Teleport(Player player)
    {
        var targetPosition = player.LastMoveTargetPosition;
        int randomIndex = UnityEngine.Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0:
                targetPosition.x = targetPosition.x + 2;
                break;
            case 1:
                targetPosition.x = targetPosition.x - 2;
                break;
            case 2:
                targetPosition.y = targetPosition.y + 2;
                break;
            case 3:
                targetPosition.y = targetPosition.y - 2;
                break;
        }
        transform.position = targetPosition;
    }
}
