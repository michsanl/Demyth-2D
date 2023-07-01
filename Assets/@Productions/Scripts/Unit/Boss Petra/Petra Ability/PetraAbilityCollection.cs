using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using Sirenix.OdinInspector;

public class PetraAbilityCollection : SceneService
{
    
    protected bool isBusy;
    
    private PetraAbilityUpCharge abilityUpCharge;
    private PetraAbilityDownCharge abilityDownCharge;
    private PetraAbilityHorizontalCharge abilityHorizontalCharge;
    private PetraAbilitySpinAttack abilitySpinAttack;
    private PetraAbilityChargeAttack abilityChargeAttack;
    private PetraAbilityBasicSlam abilityBasicSlam;
    private PetraAbilityJumpSlam abilityJumpSlam;
    private LookOrientation lookOrientation;
    protected Health health;

    protected void Awake()
    {
        abilityUpCharge = GetComponent<PetraAbilityUpCharge>();
        abilityDownCharge = GetComponent<PetraAbilityDownCharge>();
        abilityHorizontalCharge = GetComponent<PetraAbilityHorizontalCharge>();
        abilitySpinAttack = GetComponent<PetraAbilitySpinAttack>();
        abilityChargeAttack = GetComponent<PetraAbilityChargeAttack>();
        abilityBasicSlam = GetComponent<PetraAbilityBasicSlam>();
        abilityJumpSlam = GetComponent<PetraAbilityJumpSlam>();

        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
    }

#region Ability Collection

    protected IEnumerator PlayAbilityUpCharge()
    {
        isBusy = true;
        yield return StartCoroutine(abilityUpCharge.UpCharge());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityDownCharge()
    {
        isBusy = true;
        yield return StartCoroutine(abilityDownCharge.DownCharge());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalCharge()
    {
        isBusy = true;
        yield return StartCoroutine(abilityHorizontalCharge.HorizontalCharge());
        isBusy = false;
    }

    protected IEnumerator PlayAbilitySpinAttack()
    {
        isBusy = true;
        yield return StartCoroutine(abilitySpinAttack.SpinAttack());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityChargeAttack()
    {
        isBusy = true;
        yield return StartCoroutine(abilityChargeAttack.ChargeAttack());
        isBusy = false;
    }

    protected IEnumerator PlayAbilityBasicSlam()
    {
        isBusy = true;
        yield return StartCoroutine(abilityBasicSlam.BasicSlam());
        isBusy = false;
    }
    
    protected IEnumerator PlayAbilityJumpSlam()
    {
        isBusy = true;
        yield return StartCoroutine(abilityJumpSlam.JumpSlam());
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
