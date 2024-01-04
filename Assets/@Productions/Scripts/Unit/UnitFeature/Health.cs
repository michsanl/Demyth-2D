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

    public int MaxHP => maxHealth;
    public int CurrentHP 
    { 
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (value <= 0) OnDeath?.Invoke();
        }
    }

    [Header("Status")]
    [SerializeField]
    private HealthStatus status = HealthStatus.Normal;

    public Action OnTakeDamage;
    public Action OnHealthChanged;
    public Action OnDeath;

    private void Start()
    {
        ResetHealthToMaximum();
    }

    [Button("Reset Health", ButtonSizes.Medium)]
    public void ResetHealthToMaximum()
    {
        CurrentHP = maxHealth;

        OnHealthChanged?.Invoke();
    }

    public void TakeDamage()
    {
        if (status == HealthStatus.Invulnerable) 
            return;

        if (currentHealth <= 0)
            return;

        CurrentHP--;

        OnHealthChanged?.Invoke();
        OnTakeDamage?.Invoke();
    }

    public void Heal()
    {
        CurrentHP++;

        OnHealthChanged?.Invoke();
    }

    private void Death()
    {
        OnDeath?.Invoke();
    }

    public bool IsHealthFull()
    {
        return currentHealth == maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}

public enum HealthStatus
{
    Normal, Invulnerable
}

