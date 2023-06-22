using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;

public class SriAbilityTeleport : SceneService
{
    [SerializeField] private float teleportStartDuration;
    [SerializeField] private float teleportEndDuration;
    [SerializeField] private Animator animator;
    
    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");

    public IEnumerator Teleport()
    {
        animator.Play(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(teleportStartDuration);

        var teleportTargetPosition = GetTeleportTargetPosition();
        transform.position = teleportTargetPosition;

        animator.Play(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(teleportEndDuration);
    }

    private Vector2 GetTeleportTargetPosition()
    {
        Vector3 targetPosition = Context.Player.LastMoveTargetPosition;
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

        if (targetPosition == transform.position)
        {
            return GetTeleportTargetPosition();
        }
        else
        {
            return targetPosition; 
        }
    }
}
