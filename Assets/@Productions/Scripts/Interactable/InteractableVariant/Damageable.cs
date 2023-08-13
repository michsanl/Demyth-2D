using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Damageable : Interactable
{
    public static Action OnAnyDamageableInteract;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public override void Interact(Player player, Vector3 dir = default)
    {
        OnAnyDamageableInteract?.Invoke();
        health.TakeDamage();
    }
}
