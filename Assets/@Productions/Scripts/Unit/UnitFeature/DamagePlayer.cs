using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using CustomTools.Core;

public class DamagePlayer : SceneService
{
    public DamagerCharacter damagerCharacter;

    [Title("KnockBack Settings")]
    [SerializeField] private bool isKnockbackOnly;
    [SerializeField] private bool isKnockbackPassThrough;
    [SerializeField] private bool enableKnockBack = true;
    [ShowIf("enableKnockBack")]
    public KnockBackSource knockBackSource;
    [ShowIf("enableKnockBack")]
    public KnockBackOrientation knockBackOrientation;
    [ShowIf("knockBackOrientation", KnockBackOrientation.SelectedDir)]
    public KnockBackDirection knockBackDirection;

    [Title("Layer Mask Input")]
    [SerializeField] private LayerMask moveBlockMask;
    [SerializeField] private LayerMask damagePlayerMask;

    public enum KnockBackOrientation { AllDir, HorizontalDir, VerticalDir, SelectedDir };
    public enum KnockBackDirection { Up, Down, Left, Right }
    public enum KnockBackSource { ThisObject, Player }
    public enum DamagerCharacter { NotSet, Petra, Sri }

    private Player player;
    private Vector3 knockBackOrigin;
    private Vector2 knockBackDir;
    private Vector2 knockBackTargetPosition;

    private void OnCollisionStay(Collision other) 
    {
        if (player != null)
        {
            if (isKnockbackOnly)
            {
                player.TriggerKnockBack(knockBackTargetPosition);
            }
            else
            {
                player.TakeDamage(enableKnockBack, knockBackTargetPosition, damagerCharacter);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        player = other.collider.GetComponent<Player>();
        knockBackOrigin = GetKnockBackOrigin();

        SetKnockbackTargetPosition();
    }

    private void SetKnockbackTargetPosition()
    {
        if (enableKnockBack)
        {
            switch (knockBackOrientation)
            {
                case KnockBackOrientation.AllDir:
                    knockBackDir = GetAllKnockBackDir();
                    knockBackTargetPosition = GetKnockBackTargetPosition(knockBackDir);
                    break;
                case KnockBackOrientation.SelectedDir:
                    knockBackDir = GetSelectedKnockBackDir();
                    knockBackTargetPosition = GetKnockBackTargetPosition(knockBackDir);
                    break;
                case KnockBackOrientation.HorizontalDir:
                    knockBackDir = GetHorizontalKnockBackDir();
                    knockBackTargetPosition = GetKnockBackTargetPosition(knockBackDir);
                    break;
                case KnockBackOrientation.VerticalDir:
                    knockBackDir = GetVerticalKnockBackDir();
                    knockBackTargetPosition = GetKnockBackTargetPosition(knockBackDir);
                    break;
                default:
                    break;
            }
        }
    }

    private Vector2 GetAllKnockBackDir()
    {
        Vector2 knockBackDir = GetDirectionToPlayer();

        if (knockBackDir == Vector2.zero)
            knockBackDir = player.PlayerDir;

        int loopCount = 4;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir, knockBackOrigin))
                return knockBackDir;
            
            knockBackDir = Vector2.Perpendicular(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector2 GetVerticalKnockBackDir()
    {
        Vector2 knockBackDir = GetInitialVerticalKnockBackDir();

        int loopCount = 2;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir, knockBackOrigin))
                return knockBackDir;

            knockBackDir = GetOppositeDirection(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector2 GetHorizontalKnockBackDir()
    {
        Vector2 knockBackDir = GetInitialHorizontalKnockBackDir();

        int loopCount = 2;
        for (int i = 0; i < loopCount; i++)
        {
            if (!IsDirectionBlocked(knockBackDir, knockBackOrigin))
                return knockBackDir;

            knockBackDir = GetOppositeDirection(knockBackDir);
        }
        return Vector2.zero;
    }

    private Vector2 GetSelectedKnockBackDir()
    {
        Vector2 selectedDir = GetSelectedDir();

        if (isKnockbackPassThrough)
        {
            return selectedDir;
        }
        else
        {
            return IsDirectionBlocked(selectedDir, transform.position) ? Vector2.zero : selectedDir;
        }
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (player.transform.position - transform.position).normalized;
    }

    private Vector2 GetInitialVerticalKnockBackDir()
    {
        if (player.transform.position.y > transform.position.y)
            return Vector2.up;
        if (player.transform.position.y < transform.position.y)
            return Vector2.down;
        return UnityEngine.Random.Range(0, 2) == 0 ? Vector2.up : Vector2.down;
    }

    private Vector2 GetInitialHorizontalKnockBackDir()
    {
        if (player.transform.position.x > transform.position.x)
            return Vector2.right;
        if (player.transform.position.x < transform.position.x)
            return Vector2.left;
        return UnityEngine.Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left;
    }

    private Vector2 GetSelectedDir()
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

    private Vector3 GetKnockBackOrigin()
    {
        return knockBackSource == KnockBackSource.ThisObject ? transform.position : player.LastMoveTargetPosition;
    }

    private Vector2 GetKnockBackTargetPosition(Vector2 knockBackDir)
    {
        Vector3 knockBackOrigin = GetKnockBackOrigin();

        return new Vector2( Mathf.RoundToInt(knockBackDir.x + knockBackOrigin.x), 
            Mathf.RoundToInt(knockBackDir.y + knockBackOrigin.y));
    }

    private bool IsDirectionBlocked(Vector2 knockBackDir, Vector3 knockBackOrigin)
    {
        var moveBlockerAhead = Helper.CheckTargetDirection(knockBackOrigin, knockBackDir, moveBlockMask, out Interactable interactable);
        var damagingPlayerAhead = Physics.Raycast(knockBackOrigin, knockBackDir, 1f, damagePlayerMask); 

        return moveBlockerAhead || damagingPlayerAhead ? true : false;
    }

    private Vector2 GetOppositeDirection(Vector2 dir)
    {
        dir.x = Mathf.RoundToInt(dir.x * -1f); 
        dir.y = Mathf.RoundToInt(dir.y * -1f);
        return dir;
    }
}
