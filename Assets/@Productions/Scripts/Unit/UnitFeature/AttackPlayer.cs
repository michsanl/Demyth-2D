using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackPlayer : MonoBehaviour
{
    [Title("CameraShake Settings")]
    [SerializeField] private bool enableCameraShake = true;

    [Title("KnockBack Settings")]
    [SerializeField] private bool enableKnockBack = true;
    [ShowIf("enableKnockBack")]
    public KnockBackDirection knockBackDirection;
    public enum KnockBackDirection { Up, Down, Left, Right }

    private Player player;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        player = other.collider.GetComponent<Player>();
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        StartCoroutine(player.DamagePlayer(enableCameraShake, enableKnockBack, GetKnockBackDirection()));
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
