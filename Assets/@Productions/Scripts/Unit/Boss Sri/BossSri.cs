using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class BossSri : SceneService
{

    private BossSriFirstPhase bossSriFirstPhase;
    private BossSriSecondPhase bossSriSecondPhase;
    private Health health;

    protected override void OnInitialize()
    {
        bossSriFirstPhase = GetComponent<BossSriFirstPhase>();
        bossSriSecondPhase = GetComponent<BossSriSecondPhase>();
        health = GetComponent<Health>();
    }

    protected override void OnActivate()
    {
        health.OnAfterTakeDamage += Health_OnTakeDamage;

        StartFirstPhase();
    }

    private void Health_OnTakeDamage()
    {
        if (health.CurrentHP == 10)
        {
            StartSecondPhase();
        }
    }

    private void StartFirstPhase()
    {
        bossSriFirstPhase.IsBehaviorActive = true;
        bossSriSecondPhase.IsBehaviorActive = false;
    }

    private void StartSecondPhase()
    {
        bossSriFirstPhase.IsBehaviorActive = false;
        bossSriSecondPhase.IsBehaviorActive = true;
    }
}
