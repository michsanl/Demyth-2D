using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackApplier : MonoBehaviour
{
    public KnockBackDirection knockBackDirection;
    public enum KnockBackDirection { Up, Down, Left, Right }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Player player = other.collider.GetComponent<Player>();
        
        switch (knockBackDirection)
        {
            case KnockBackDirection.Up:
                StartCoroutine(player.KnockBack(Vector2.up));
                break;
            case KnockBackDirection.Down:
                StartCoroutine(player.KnockBack(Vector2.down));
                break;
            case KnockBackDirection.Left:
                StartCoroutine(player.KnockBack(Vector2.left));
                break;
            case KnockBackDirection.Right:
                StartCoroutine(player.KnockBack(Vector2.right));
                break;
            default:
                break;
        }
    }
}
