using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;
using Sirenix.OdinInspector;

public class SriCombatBehaviorBase : SceneService
{
    protected bool isBusy;
    
    protected SriAbilityUpSlash abilityUpSlash;
    protected SriAbilityDownSlash abilityDownSlash;
    protected SriAbilityHorizontalSlash abilityHorizontalSlash;
    protected SriAbilitySpinClaw abilitySpinClaw;
    protected SriAbilityNailAOE abilityNailAOE;
    protected SriAbilityNailSummon abilityNailSummon;
    protected SriAbilityFireBall abilityFireBall;
    protected SriAbilityTeleport abilityTeleport;
    protected SriAbilityHorizontalNailWave abilityHorizontalNailWave;
    protected SriAbilityVerticalNailWave abilityVerticalNailWave;
    protected SriAbilityWaveOutNailWave abilityWaveOutNailWave;
    protected LookOrientation lookOrientation;
    protected Health health;

    protected Coroutine primaryActiveCoroutine;
    protected Coroutine secondaryActiveCoroutine;

    protected override void OnInitialize()
    {
        abilityUpSlash = GetComponent<SriAbilityUpSlash>();
        abilityDownSlash = GetComponent<SriAbilityDownSlash>();
        abilityHorizontalSlash = GetComponent<SriAbilityHorizontalSlash>();
        abilitySpinClaw = GetComponent<SriAbilitySpinClaw>();
        abilityNailAOE = GetComponent<SriAbilityNailAOE>();
        abilityNailSummon = GetComponent<SriAbilityNailSummon>();
        abilityFireBall = GetComponent<SriAbilityFireBall>();
        abilityTeleport = GetComponent<SriAbilityTeleport>();
        abilityHorizontalNailWave = GetComponent<SriAbilityHorizontalNailWave>();
        abilityVerticalNailWave = GetComponent<SriAbilityVerticalNailWave>();
        abilityWaveOutNailWave = GetComponent<SriAbilityWaveOutNailWave>();

        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
    }

#region Ability Collection

    protected IEnumerator PlayAbilityUpSlash()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityUpSlash.UpSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityDownSlash()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityDownSlash.DownSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalSlash()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityHorizontalSlash.HorizontalSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilitySpinClaw()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilitySpinClaw.SpinClaw());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailAOE()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityNailAOE.NailAOE(false));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailAOEWithProjectile()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityNailAOE.NailAOE(true));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailSummon()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityNailSummon.NailSummon(Context.Player));
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityFireBall()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityFireBall.FireBall());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalNailWave()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityHorizontalNailWave.PlayAbility());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityVerticalNailWave()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityVerticalNailWave.PlayAbility());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityWaveOutNailWave()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityWaveOutNailWave.PlayAbility());
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityTeleport()
    {
        isBusy = true;
        yield return secondaryActiveCoroutine = StartCoroutine(abilityTeleport.Teleport());
        isBusy = false;
    }

    protected IEnumerator TeleportMultipleTimes()
    {
        isBusy = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(abilityTeleport.Teleport());            
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

    protected bool IsPlayerAtSamePosX()
    {
        return Mathf.Approximately(transform.position.x, Context.Player.transform.position.x) ;
    }

    protected bool IsPlayerAtSamePosY()
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
