using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using Sirenix.OdinInspector;

public class SriAbilityCollection : SceneService
{
    protected bool isBusy;
    
    private SriAbilityUpSlash abilityUpSlash;
    private SriAbilityDownSlash abilityDownSlash;
    private SriAbilityHorizontalSlash abilityHorizontalSlash;
    private SriAbilitySpinClaw abilitySpinClaw;
    private SriAbilityNailAOE abilityNailAOE;
    private SriAbilityNailSummon abilityNailSummon;
    private SriAbilityFireBall abilityFireBall;
    private SriAbilityTeleport abilityTeleport;

    private LookOrientation lookOrientation;

    protected void Awake()
    {
        abilityUpSlash = GetComponent<SriAbilityUpSlash>();
        abilityDownSlash = GetComponent<SriAbilityDownSlash>();
        abilityHorizontalSlash = GetComponent<SriAbilityHorizontalSlash>();
        abilitySpinClaw = GetComponent<SriAbilitySpinClaw>();
        abilityNailAOE = GetComponent<SriAbilityNailAOE>();
        abilityNailSummon = GetComponent<SriAbilityNailSummon>();
        abilityFireBall = GetComponent<SriAbilityFireBall>();
        abilityTeleport = GetComponent<SriAbilityTeleport>();

        lookOrientation = GetComponent<LookOrientation>();
    }

#region Ability Collection

    protected IEnumerator PlayAbilityUpSlash()
    {
        isBusy = true;
        yield return StartCoroutine(abilityUpSlash.UpSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityDownSlash()
    {
        isBusy = true;
        yield return StartCoroutine(abilityDownSlash.DownSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalSlash()
    {
        isBusy = true;
        yield return StartCoroutine(abilityHorizontalSlash.HorizontalSlash(Context.Player));
        isBusy = false;
    }

    protected IEnumerator PlayAbilitySpinClaw()
    {
        isBusy = true;
        yield return StartCoroutine(abilitySpinClaw.SpinClaw());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailAOE()
    {
        isBusy = true;
        yield return StartCoroutine(abilityNailAOE.NailAOE());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityNailSummon()
    {
        isBusy = true;
        yield return StartCoroutine(abilityNailSummon.NailSummon(Context.Player));
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityFireBall()
    {
        isBusy = true;
        yield return StartCoroutine(abilityFireBall.FireBall());
        isBusy = false;
    }
    
    protected void PlayAbilityTeleport()
    {
        abilityTeleport.Teleport(Context.Player);
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
