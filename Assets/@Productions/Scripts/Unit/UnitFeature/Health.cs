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

            if (value <= 0)
            {
                Death();
            }
        }
    }

    [Header("Status")]
    [SerializeField]
    private HealthStatus status = HealthStatus.Normal;

    public Action OnTakeDamage;
    public Action OnHealthChanged;

    private Shield shield;

    private void Awake() 
    {
        shield = GetComponent<Shield>();
    }

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
        if (shield != null)
        {
            if (shield.TryShieldTakeDamage())
                return;
        }

        CurrentHP--;

        OnHealthChanged?.Invoke();
        OnTakeDamage?.Invoke();
    }

    public void Heal(int healAmount)
    {
        CurrentHP++;

        OnHealthChanged?.Invoke();
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

