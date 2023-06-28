using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Health : MonoBehaviour
{
    [Header("Health Attribute")]
    [SerializeField]
    private int maxHealth = 5;
    [SerializeField, ReadOnly]
    private int currentHealth;
    public int CurrentHP 
    { 
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            if (value <= 0)
            {
                Death();
            }
        }
    }

    [Header("Status")]
    [SerializeField]
    private HealthStatus status = HealthStatus.Normal;

    public Action OnAfterTakeDamage;

    private void Start()
    {
        ResetHealthToMaximum();
    }

    [Button("Reset Health", ButtonSizes.Medium)]
    public void ResetHealthToMaximum()
    {
        CurrentHP = maxHealth;
    }

    public void TakeDamage(int hitDamage)
    {
        if (status == HealthStatus.Invulnerable) return;

        CurrentHP--;

        OnAfterTakeDamage?.Invoke();
    }

    public void Heal(int healAmount)
    {
        CurrentHP++;
    }

    private void Death()
    {
        //Dead
        gameObject.SetActive(false);
    }

    public bool IsHealthFull()
    {
        return currentHealth == maxHealth;
    }
}

public enum HealthStatus
{
    Normal, Invulnerable
}

