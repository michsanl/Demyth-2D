using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class AttackPlayer : MonoBehaviour
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

    public enum KnockBackType { Directive, Auto, Horizontal, Vertical };
    public enum KnockBackDirection { Up, Down, Left, Right }

    private Vector2[] verticalDirArray = new Vector2[2] { Vector2.up, Vector2.down };
    private Vector2[] horizontalDirArray = new Vector2[2] { Vector2.right, Vector2.left };
    private Player player;
    private Vector2 dirToPlayer;
    private Vector2 knockBackDir;
    private Vector2 knockBackTargetPosition;

    private void OnCollisionEnter(Collision other) 
    {
        player = other.collider.GetComponent<Player>();
        dirToPlayer = (other.transform.position - transform.position).normalized;

        if (enableKnockBack)
        {
            switch (knockBackType)
            {
                case KnockBackType.Auto:
                    knockBackDir = GetAutoKnockBackDir(dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(knockBackDir);
                    break;
                case KnockBackType.Directive:
                    knockBackDir = GetDirectiveKnockBackDir();
                    knockBackTargetPosition = GetKnockBackPosition(knockBackDir);
                    break;
                case KnockBackType.Horizontal:
                    knockBackDir = GetHorizontalKnockBackDir(dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(knockBackDir);
                    break;
                case KnockBackType.Vertical:
                    knockBackDir = GetVerticalKnockBackDir(dirToPlayer);
                    knockBackTargetPosition = GetKnockBackPosition(knockBackDir);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision other) 
    {
        player.TakeDamage();

        if (enableKnockBack)
            player.TriggerKnockBack(knockBackTargetPosition);
    }

    private Vector2 GetKnockBackPosition(Vector2 knockBackDir)
    {
        return new Vector2( Mathf.RoundToInt(knockBackDir.x + transform.position.x), 
            Mathf.RoundToInt(knockBackDir.y + transform.position.y));
    }

    private Vector2 GetVerticalKnockBackDir(Vector2 direction)
    {
        if (!verticalDirArray.Contains(direction))
            direction = Vector2.up;

        int checkDiretionLoopCount = 2;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(transform.position, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(transform.position, direction, 1f, damagePlayerMask); 

            if (noMoveBlockerAhead && noDamagingPlayerAhead)
            {
                return direction;
            }
            direction = GetOppositeDirection(direction);
        }
        return Vector2.zero;
    }

    private Vector2 GetHorizontalKnockBackDir(Vector2 direction)
    {
        if (!horizontalDirArray.Contains(direction))
            direction = Vector2.right;

        int checkDiretionLoopCount = 2;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(transform.position, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(transform.position, direction, 1f, damagePlayerMask); 

            if (noMoveBlockerAhead && noDamagingPlayerAhead)
            {
                return direction;
            }
            direction = GetOppositeDirection(direction);
        }
        return Vector2.zero;
    }

    private Vector2 GetAutoKnockBackDir(Vector2 direction)
    {
        if (direction == Vector2.zero)
            direction = Vector2.up;

        int checkDiretionLoopCount = 4;
        for (int i = 0; i < checkDiretionLoopCount; i++)
        {
            var noMoveBlockerAhead = !Helper.CheckTargetDirection(transform.position, direction, moveBlockMask, out Interactable interactable);
            var noDamagingPlayerAhead = !Physics.Raycast(transform.position, direction, 1f, damagePlayerMask); 

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
        switch (knockBackDirection)
        {
            case KnockBackDirection.Up:
                return Vector2.up;
            case KnockBackDirection.Down:
                return Vector2.down;
            case KnockBackDirection.Left:
                return Vector2.left;
            case KnockBackDirection.Right:
                return Vector2.right;
            default:
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
