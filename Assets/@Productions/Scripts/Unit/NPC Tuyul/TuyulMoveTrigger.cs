using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuyulMoveTrigger : MonoBehaviour
{

    public Direction direction;
    public enum Direction { Up, Down, Right, Left }

    [SerializeField] private TuyulMovement tuyulMovement;

    private Vector3 moveDir;

    private void Awake() 
    {
        switch (direction)
        {
            case Direction.Up:
                moveDir = Vector3.up;
                break;
            case Direction.Down:
                moveDir = Vector3.down;
                break;
            case Direction.Right:
                moveDir = Vector3.right;
                break;
            case Direction.Left:
                moveDir = Vector3.left;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        tuyulMovement.StartCoroutine(tuyulMovement.Flee(moveDir));
    }

    private void OnCollisionEnter(Collision other) 
    {
        tuyulMovement.StartCoroutine(tuyulMovement.Flee(moveDir));
    }
}
