using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SriCombatBehaviorController : MonoBehaviour
{
    
    private SriCombatBehaviorBase[] sriCombatBehaviorArray;
    private Health health;
    private bool isPhaseTwoActive;

    private void Awake() 
    {
        sriCombatBehaviorArray = GetComponents<SriCombatBehaviorBase>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.OnAfterTakeDamage += Health_OnAfterTakeDamage;

        ActivatePhaseOne();
    }

    private void Health_OnAfterTakeDamage()
    {
        if (health.CurrentHP <= 10)
        {
            ActivatePhaseTwo();
        }
    }

    private void ActivatePhaseOne()
    {
        foreach (var combatBehavior in sriCombatBehaviorArray)
        {
            if (combatBehavior is SriCombatBehaviorPhaseOne)
            {
                combatBehavior.SetIsActive(true);
            }
            else
            {
                combatBehavior.SetIsActive(false);
            }
        }
    }

    private void ActivatePhaseTwo()
    {
        if (isPhaseTwoActive)
            return;

        foreach (var combatBehavior in sriCombatBehaviorArray)
        {
            if (combatBehavior is SriCombatBehaviorPhaseTwo)
            {
                combatBehavior.SetIsActive(true);
            }
            else
            {
                combatBehavior.SetIsActive(false);
            }
        }

        isPhaseTwoActive = true;
    }
}
