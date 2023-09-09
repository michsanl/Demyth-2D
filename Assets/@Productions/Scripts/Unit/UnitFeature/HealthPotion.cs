using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;

public class HealthPotion : SceneService
{
    [Header("Potion Attribute")]
    [SerializeField]
    private int maxPotionAmount = 2;
    private float potionCooldown;
    [SerializeField, ReadOnly]
    private int currentPotionAmount;
    private bool isHealthPotionOnCooldown;

    public Action<int> OnPotionAmountChanged;
    public int CurrentPotionAmount => currentPotionAmount;
    public bool IsHealthPotionOnCooldown => isHealthPotionOnCooldown;

    private Health health;

    protected override void OnInitialize()
    {
        health = GetComponent<Health>();
    }

    protected override void OnActivate()
    {
        ResetPotionToMax();
    }

    public void UsePotion()
    {
        currentPotionAmount--;
        health.Heal(1); 
        StartCoroutine(StartPotionCooldown());
        
        OnPotionAmountChanged?.Invoke(currentPotionAmount);
    }

    private IEnumerator StartPotionCooldown()
    {
        isHealthPotionOnCooldown = true;
        yield return Helper.GetWaitForSeconds(potionCooldown);
        isHealthPotionOnCooldown = false;
    }

    [Button("Reset Potion", ButtonSizes.Medium)]
    public void ResetPotionToMax()
    {
        currentPotionAmount = maxPotionAmount;
        OnPotionAmountChanged?.Invoke(currentPotionAmount);
    }
}

