using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Damageable : Interactable
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public override void Interact(Vector3 dir = default)
    {
        if (health == null) return;
        
        health.TakeDamage(1);
    }
}
