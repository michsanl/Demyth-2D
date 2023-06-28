using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class HealthPotion : MonoBehaviour
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
        if (currentPotionAmount <= 0) 
            return;
        if (isHealthPotionOnCooldown)
            return;
        if (health.IsHealthFull())
            return;
            
        currentPotionAmount--;
        health.Heal(1);
        OnPotionAmountChanged?.Invoke(currentPotionAmount);

        StartCoroutine(StartPotionCooldown());
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

