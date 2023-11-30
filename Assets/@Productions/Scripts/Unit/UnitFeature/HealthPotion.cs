using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using CustomTools.Core;

public class HealthPotion : MonoBehaviour
{
    public int CurrentPotionAmount => currentPotionAmount;
    public bool IsHealthPotionOnCooldown => isHealthPotionOnCooldown;

    [Header("Potion Attribute")]
    [SerializeField]
    private int maxPotionAmount = 2;
    private float potionCooldown;
    [SerializeField, ReadOnly]
    private int currentPotionAmount;
    private bool isHealthPotionOnCooldown;

    public Action<int> OnPotionAmountChanged;    

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
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

