using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using CustomTools.Core;

public class AttackPlayer : SceneService
{
    [Title("KnockBack Settings")]
    [SerializeField] private bool enableKnockBack = true;
    [ShowIf("enableKnockBack")]
    public KnockBackType knockBackType;
    [ShowIf("knockBackType", KnockBackType.Directive)]
    public KnockBackDirection knockBackDirection;

    [Title("Layer Mask Input")]
    [SerializeField] private LayerMask moveBlockMask;
    [SerializeField] private LayerMask damagePlayerMask;

    public enum KnockBackType { Directive, Auto, Horizontal, Vertical, HrzSlash, VrtSlash };
    public enum KnockBackDirection { Up, Down, Left, Right }

    private Vector2[] verticalDirArray = new Vector2[2] { Vector2.up, Vector2.down };
    private Vector2[] horizontalDirArray = new Vector2[2] { Vector2.right, Vector2.left };
    private Player player;
    private Vector2 dirToPlayer;
    private Vector2 knockBackDir;
    private Vector2 knockBackTargetPosition;

    private void OnCollisionStay(Collision other) 
    {
        if (player != null)
            player.TakeDamage(enableKnockBack, knockBackTargetPosition);
    }

    private void OnCollisionEnter(Collision other) 
    {
        player = other.collider.GetComponent<Player>();
        dirToPlayer = (other.transform.position - transform.position).normalized;

        if (enableKnockBack)
        {
            switch (knockBackType)
            {
                case KnockBackType.Auto:
                    knockBackDir = GetKnockBackAllDir(transform.position, dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(transform.position, knockBackDir);
                    break;
                case KnockBackType.Directive:
                    knockBackDir = GetDirectiveKnockBackDir();
                    knockBackTargetPosition = GetKnockBackPosition(transform.position, knockBackDir);
                    break;
                case KnockBackType.Horizontal:
                    knockBackDir = GetKnockBackHorizontalDir(transform.position, dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(transform.position, knockBackDir);
                    break;
                case KnockBackType.Vertical:
                    knockBackDir = GetKnockBackVerticalDir(transform.position, dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(transform.position, knockBackDir);
                    break;
                case KnockBackType.HrzSlash:
                    knockBackDir = GetKnockBackHorizontalDir(Context.Player.transform.position, dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(Context.Player.transform.position, knockBackDir);
                    break;
                case KnockBackType.VrtSlash:
                    knockBackDir = GetKnockBackVerticalDir(Context.Player.transform.position, dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(Context.Player.transform.position, knockBackDir);
                    break;
                default:
                    break;
            }
        }
    }

    private Vector2 GetKnockBackPosition(Vector3 knockBackOrigin, Vector2 knockBackDir)
    {
        return new Vector2( Mathf.RoundToInt(knockBackDir.x + knockBackOrigin.x), 
            Mathf.RoundToInt(knockBackDir.y + knockBackOrigin.y));
    }

    private Vector2 GetKnockBackVerticalDir(Vector3 knockBackOrigin, Vector2 direction)
    {
        if (!verticalDirArray.Contains(direction))
            direction = UnityEngine.Random.Range(0, 2) == 0 ? Vector2.up : Vector2.down;

        int checkDiretionLoopCount = 2;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(knockBackOrigin, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(knockBackOrigin, direction, 1f, damagePlayerMask); 

            if (noMoveBlockerAhead && noDamagingPlayerAhead)
            {
                return direction;
            }
            direction = GetOppositeDirection(direction);
        }
        return Vector2.zero;
    }

    private Vector2 GetKnockBackHorizontalDir(Vector3 knockBackOrigin, Vector2 direction)
    {
        if (!horizontalDirArray.Contains(direction))
            direction = UnityEngine.Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;

        int checkDiretionLoopCount = 2;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(knockBackOrigin, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(knockBackOrigin, direction, 1f, damagePlayerMask); 

            if (noMoveBlockerAhead && noDamagingPlayerAhead)
            {
                return direction;
            }
            direction = GetOppositeDirection(direction);
        }
        return Vector2.zero;
    }

    private Vector2 GetKnockBackAllDir(Vector3 knockBackOrigin, Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            direction = player.LastPlayerDir;
        }

        int checkDiretionLoopCount = 4;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(knockBackOrigin, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(knockBackOrigin, direction, 1f, damagePlayerMask); 

            if (noMoveBlockerAhead && noDamagingPlayerAhead)
            {
                return direction;
            }
            Vector2.Perpendicular(direction);
        }
        return Vector2.zero;
    }

    private Vector2 GetDirectiveKnockBackDir()
    {
        Vector2 direction;

        switch (knockBackDirection)
        {
            case KnockBackDirection.Up:
                direction = Vector2.up;
                break;
            case KnockBackDirection.Down:
                direction = Vector2.down;
                break;
            case KnockBackDirection.Left:
                direction = Vector2.left;
                break;
            case KnockBackDirection.Right:
                direction = Vector2.right;
                break;
            default:
                direction = Vector2.zero;
                break;
        }

        if (!Helper.CheckTargetDirection(transform.position, direction, moveBlockMask, out Interactable interactable))
        {
            return direction;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private Vector2 GetOppositeDirection(Vector2 dir)
    {
        dir.x = Mathf.RoundToInt(dir.x * -1f); 
        dir.y = Mathf.RoundToInt(dir.y * -1f);
        return dir;
    }
}
