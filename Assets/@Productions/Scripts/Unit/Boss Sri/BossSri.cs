using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class BossSri : SceneService
{
    [SerializeField] private bool activateAbilityTester;

    private BossSriFirstPhase bossSriFirstPhase;
    private BossSriSecondPhase bossSriSecondPhase;
    private BossSriAbilityTester bossSriAbilityTester;
    private Health health;

    protected override void OnInitialize()
    {
        bossSriFirstPhase = GetComponent<BossSriFirstPhase>();
        bossSriSecondPhase = GetComponent<BossSriSecondPhase>();
        bossSriAbilityTester = GetComponent<BossSriAbilityTester>();
        health = GetComponent<Health>();
    }

    protected override void OnActivate()
    {
        health.OnAfterTakeDamage += Health_OnTakeDamage;

        if (activateAbilityTester)
        {
            bossSriAbilityTester.ActivateTester = true;
        }
        else
        {
            StartFirstPhase();
        }
    }

    private void Health_OnTakeDamage()
    {
        if (health.CurrentHP == 10)
        {
            if (activateAbilityTester)
                return;

            StartSecondPhase();
        }
    }

    private void StartFirstPhase()
    {
        bossSriFirstPhase.ActivateFirstPhase = true;
        bossSriSecondPhase.ActivateSecondPhase = false;
    }

    private void StartSecondPhase()
    {
        bossSriFirstPhase.ActivateFirstPhase = false;
        bossSriSecondPhase.ActivateSecondPhase = true;
    }
}
