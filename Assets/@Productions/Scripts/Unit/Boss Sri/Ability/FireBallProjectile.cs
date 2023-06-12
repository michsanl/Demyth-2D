using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;

public class FireBallProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lifeSpanDuration;

    private Player player;

    private void Awake() 
    {
        player = FindObjectOfType<Player>();

        var dir = player.transform.position - transform.position;
        transform.up = dir;

        Destroy(gameObject, lifeSpanDuration);
    }

    private void Update() 
    {
        var dir = player.transform.position - transform.position;

        transform.up = Vector3.MoveTowards(transform.up, dir, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, moveSpeed * Time.deltaTime);
    }


}
