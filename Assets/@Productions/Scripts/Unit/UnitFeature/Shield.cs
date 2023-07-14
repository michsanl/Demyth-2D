using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Shield : MonoBehaviour
{
    [Header("Shield Attribute")]
    [SerializeField] private int maxShield = 5;
    [SerializeField] private float shieldRegenSpeed = 0.25f;
    [SerializeField, ReadOnly] private float currentShield;

    public int MaxShield => maxShield;
    public float CurrentShield 
    { 
        get => Mathf.Clamp(currentShield, 0, maxShield);
        set
        {
            currentShield = Mathf.Clamp(value, 0, maxShield);
        }
    }

    public Action OnShieldAmountChanged;

    private void Start() 
    {
        ResetShieldToMaximum();
    }

    private void Update()
    {
        ShieldRegeneration();
    }

    private void ShieldRegeneration()
    {
        if (currentShield < maxShield)
        {
            currentShield += Time.deltaTime * shieldRegenSpeed;

            OnShieldAmountChanged?.Invoke();
        }
    }

    [Button("Reset Shield", ButtonSizes.Medium)]
    public void ResetShieldToMaximum()
    {
        CurrentShield = maxShield;

        OnShieldAmountChanged?.Invoke();
    }

    public bool TryShieldTakeDamage()
    {
        if (currentShield < 1)
        {
            return false;
        }
        else
        {
            ShieldTakeDamage();
            return true;
        }
    }

    private void ShieldTakeDamage()
    {
        CurrentShield--;

        OnShieldAmountChanged?.Invoke();
    }

    public bool IsShieldFull()
    {
        return currentShield == maxShield;
    }
}
