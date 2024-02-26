using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using Core;
using Demyth.Gameplay;

public class ProjectileHomingSingle : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lifeSpanDuration;
    [SerializeField] private float homingDuration;
    [SerializeField] private float idleDuration;

    private Player player;
    private float homingTimer;
    private float idleTimer;

    private void Awake() 
    {
        player = SceneServiceProvider.GetService<PlayerManager>().Player;

        homingTimer = homingDuration;
        idleTimer = idleDuration;

        StartCoroutine(StartDestroySelfCountdown());
    }

    private void Start()
    {
        var dir = player.transform.position - transform.position;
        transform.up = dir;
    }

    private void Update()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer > 0)
            return;

        homingTimer -= Time.deltaTime;
        if (homingTimer > 0)
        {
            ChangeDirTowardsPlayer();
        }

        MoveTowardsDir();
    }

    private void OnEnable()
    {
        var dir = player.transform.position - transform.position;
        transform.up = dir;
        
        StartCoroutine(StartDestroySelfCountdown());
    }

    private void OnDisable() 
    {
        homingTimer = homingDuration;
        idleTimer = idleDuration;

        StopAllCoroutines();
    }

    private IEnumerator StartDestroySelfCountdown()
    {
        yield return Helper.GetWaitForSeconds(lifeSpanDuration);
        LeanPool.Despawn(gameObject);
    }

    private void ChangeDirTowardsPlayer()
    {
        var dir = player.transform.position - transform.position;
        transform.up = Vector3.MoveTowards(transform.up, dir, rotationSpeed * Time.deltaTime);
    }

    private void MoveTowardsDir()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, moveSpeed * Time.deltaTime);
    }
}
