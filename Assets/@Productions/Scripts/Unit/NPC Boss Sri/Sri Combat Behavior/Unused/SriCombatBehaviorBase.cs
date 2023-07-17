using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;
using Sirenix.OdinInspector;

public class SriCombatBehaviorBase : SceneService
{
    protected bool isBusy;
    
    protected SriAbilityUpSlash upSlashAbility;
    protected SriAbilityDownSlash downSlashAbility;
    protected SriAbilityHorizontalSlash horizontalSlashAbility;
    protected SriAbilitySpinClaw spinClawAbility;
    protected SriAbilityNailAOE nailAOEAbility;
    protected SriAbilityNailSummon nailSummonAbility;
    protected SriAbilityFireBall fireBallAbility;
    protected SriAbilityTeleport teleportAbility;
    protected SriAbilityHorizontalNailWave horizontalNailWaveAbility;
    protected SriAbilityVerticalNailWave verticalNailWaveAbility;
    protected SriAbilityWaveOutNailWave waveOutNailWaveAbility;
    protected LookOrientation lookOrientation;
    protected Health health;

    protected Coroutine abilityPlayerCoroutine;
    protected Coroutine abilityCoroutine;

    protected override void OnInitialize()
    {
        upSlashAbility = GetComponent<SriAbilityUpSlash>();
        downSlashAbility = GetComponent<SriAbilityDownSlash>();
        horizontalSlashAbility = GetComponent<SriAbilityHorizontalSlash>();
        spinClawAbility = GetComponent<SriAbilitySpinClaw>();
        nailAOEAbility = GetComponent<SriAbilityNailAOE>();
        nailSummonAbility = GetComponent<SriAbilityNailSummon>();
        fireBallAbility = GetComponent<SriAbilityFireBall>();
        teleportAbility = GetComponent<SriAbilityTeleport>();
        horizontalNailWaveAbility = GetComponent<SriAbilityHorizontalNailWave>();
        verticalNailWaveAbility = GetComponent<SriAbilityVerticalNailWave>();
        waveOutNailWaveAbility = GetComponent<SriAbilityWaveOutNailWave>();

        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
    }

#region Ability Collection

    protected IEnumerator PlayAbilityUpSlash()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(upSlashAbility.UpSlash());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityDownSlash()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(downSlashAbility.DownSlash());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalSlash()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(horizontalSlashAbility.HorizontalSlash());
        isBusy = false;
    }

    protected IEnumerator PlayAbilitySpinClaw()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(spinClawAbility.SpinClaw());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailAOE()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(nailAOEAbility.NailAOE());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailSummon()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(nailSummonAbility.NailSummon());
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityFireBall()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(fireBallAbility.FireBall());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalNailWave()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(horizontalNailWaveAbility.HorizontalNailWave());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityVerticalNailWave()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(verticalNailWaveAbility.VerticalNailWave());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityWaveOutNailWave()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(waveOutNailWaveAbility.WaveOutNailWave());
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityTeleport()
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(teleportAbility.Teleport());
        isBusy = false;
    }

    protected IEnumerator PlayAbility(IEnumerator ability)
    {
        isBusy = true;
        yield return abilityCoroutine = StartCoroutine(ability);
        isBusy = false;
    }

    protected IEnumerator TeleportMultipleTimes()
    {
        isBusy = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(teleportAbility.Teleport());            
            SetFacingDirection();
        }

        isBusy = false;
    }



#endregion

#region PlayerToBossPositionInfo
    protected bool IsPlayerAbove()
    {
        return transform.position.y < Context.Player.transform.position.y;
    }

    protected bool IsPlayerBelow()
    {
        return transform.position.y > Context.Player.transform.position.y;
    }

    protected bool IsPlayerToRight()
    {
        return transform.position.x < Context.Player.transform.position.x;
    }

    protected bool IsPlayerToLeft()
    {
        return transform.position.x > Context.Player.transform.position.x;
    }

    protected bool IsPlayerInlineVertically()
    {
        return Mathf.Approximately(transform.position.x, Context.Player.transform.position.x) ;
    }

    protected bool IsPlayerInlineHorizontally()
    {
        return Mathf.Approximately(transform.position.y, Context.Player.transform.position.y);
    }

    protected bool IsPlayerNearby()
    {
        return Vector2.Distance(transform.position, Context.Player.transform.position) < 1.5f;
    }
#endregion

    protected void SetFacingDirection()
    {
        if (IsPlayerToRight())
        {
            lookOrientation.SetFacingDirection(Vector2.right);
        }

        if (IsPlayerToLeft())
        {
            lookOrientation.SetFacingDirection(Vector2.left);
        }
    }

}
