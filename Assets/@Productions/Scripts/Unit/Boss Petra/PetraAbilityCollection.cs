using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using Sirenix.OdinInspector;

public class PetraAbilityCollection : SceneService
{
    [SerializeField] private bool enableAbilityTester;
    
    [ShowIf("enableAbilityTester")]
    [EnumToggleButtons]
    public Ability loopAbility;
    public enum Ability
    { UpCharge, DownCharge, HorizontalCharge, SpinAttack, ChargeAttack, BasicSlam, JumpSlam }
    
    protected bool isBusy;

    private PetraAbilityUpCharge abilityUpCharge;
    private PetraAbilityDownCharge abilityDownCharge;
    private PetraAbilityHorizontalCharge abilityHorizontalCharge;
    private PetraAbilitySpinAttack abilitySpinAttack;
    private PetraAbilityChargeAttack abilityChargeAttack;
    private PetraAbilityBasicSlam abilityBasicSlam;
    private PetraAbilityJumpSlam abilityJumpSlam;

    private LookOrientation lookOrientation;

    protected override void OnActivate()
    {
        abilityUpCharge = GetComponent<PetraAbilityUpCharge>();
        abilityDownCharge = GetComponent<PetraAbilityDownCharge>();
        abilityHorizontalCharge = GetComponent<PetraAbilityHorizontalCharge>();
        abilitySpinAttack = GetComponent<PetraAbilitySpinAttack>();
        abilityChargeAttack = GetComponent<PetraAbilityChargeAttack>();
        abilityBasicSlam = GetComponent<PetraAbilityBasicSlam>();
        abilityJumpSlam = GetComponent<PetraAbilityJumpSlam>();

        lookOrientation = GetComponent<LookOrientation>();
    }

    protected override void OnTick()
    {
        if (!enableAbilityTester)
            return;
        if (isBusy)
            return;

        SetFacingDirection();
        HandlePlayAbility();
    }

    private void HandlePlayAbility()
    {
        switch (loopAbility)
        {
            case Ability.UpCharge:
                StartCoroutine(PlayAbilityUpCharge(transform.position.y));
                break;
            case Ability.DownCharge:
                StartCoroutine(PlayAbilityDownCharge(transform.position.y));
                break;
            case Ability.HorizontalCharge:
                StartCoroutine(PlayAbilityHorizontalCharge(transform.position.x));
                break;
            case Ability.SpinAttack:
                StartCoroutine(PlayAbilitySpinAttack());
                break;
            case Ability.ChargeAttack:
                StartCoroutine(PlayAbilityChargeAttack());
                break;
            case Ability.BasicSlam:
                StartCoroutine(PlayAbilityBasicSlam());
                break;
            case Ability.JumpSlam:
                StartCoroutine(PlayAbilityJumpSlam(Context.Player.MoveTargetPosition));
                break;
            default:
                break;
        }
    }

#region Ability Collection

    protected IEnumerator PlayAbilityUpCharge(float targetYPosition)
    {
        isBusy = true;
        yield return StartCoroutine(abilityUpCharge.UpCharge(targetYPosition));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityDownCharge(float targetYPosition)
    {
        isBusy = true;
        yield return StartCoroutine(abilityDownCharge.DownCharge(targetYPosition));
        isBusy = false;
    }

    protected IEnumerator PlayAbilityHorizontalCharge(float targetXPosition)
    {
        isBusy = true;
        yield return StartCoroutine(abilityHorizontalCharge.HorizontalCharge(targetXPosition));
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
    
    protected IEnumerator PlayAbilityJumpSlam(Vector2 targetPosition)
    {
        isBusy = true;
        yield return StartCoroutine(abilityJumpSlam.JumpSlam(targetPosition));
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
