using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Shield : MonoBehaviour
{
    [Header("Shield Attribute")]
    [SerializeField] private int maxShield = 5;
    [SerializeField, ReadOnly] private int currentShield;

    public int MaxShield => maxShield;
    public int CurrentShield 
    { 
        get => currentShield;
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

    [Button("Reset Shield", ButtonSizes.Medium)]
    public void ResetShieldToMaximum()
    {
        CurrentShield = maxShield;

        OnShieldAmountChanged?.Invoke();
    }

    public bool TryShieldTakeDamage()
    {
        if (currentShield <= 0)
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
