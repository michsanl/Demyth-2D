using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackPlayer : MonoBehaviour
{
    [Title("KnockBack Settings")]
    [SerializeField] private bool enableKnockBack = true;
    [ShowIf("enableKnockBack")]
    public KnockBackDirection knockBackDirection;
    public enum KnockBackDirection { Up, Down, Left, Right }

    private Player player;

    private void OnCollisionEnter(Collision other) 
    {
        player = other.collider.GetComponent<Player>();
    }

    private void OnCollisionStay(Collision other) 
    {
        if (enableKnockBack)
            player.KnockBack(GetKnockBackDirection());
        player.TakeDamage();
    }

    private Vector2 GetKnockBackDirection()
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
}
