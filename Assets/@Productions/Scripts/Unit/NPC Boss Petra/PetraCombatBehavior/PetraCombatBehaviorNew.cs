using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraCombatBehaviorNew : SceneService
{
    [SerializeField] private bool activateCombatBehaviorOnStart;
    [SerializeField] private int changePhaseHPThreshold;
    [SerializeField, EnumToggleButtons] private CombatMode SelectedCombatMode;
    [SerializeField, EnumToggleButtons] private Ability LoopAbility;
    [SerializeField] private GameObject[] attackColliderArray;
    
    private enum Ability 
    { UpCharge, DownCharge, HorizontalCharge, SpinAttack, ChargeAttack, BasicSlam, JumpSlam }
    private enum CombatMode 
    { FirstPhase, SecondPhase, AbilityLoop }

    private PetraAbilityUpCharge upChargeAbility;
    private PetraAbilityDownCharge downChargeAbility;
    private PetraAbilityHorizontalCharge horizontalChargeAbility;
    private PetraAbilitySpinAttack spinAttackAbility;
    private PetraAbilityChargeAttack chargeAttackAbility;
    private PetraAbilityBasicSlam basicSlamAbility;
    private PetraAbilityJumpSlam jumpSlamAbility;
    private PetraAbilityJumpGroundSlam jumpGroundSlamAbility;
    private LookOrientation lookOrientation;
    private Health health;
    private int lastRandomResult;
    private int consecutiveCount;
    private float timeVarianceCompensationDelay = 0.05f;

    protected void Awake()
    {
        upChargeAbility = GetComponent<PetraAbilityUpCharge>();
        downChargeAbility = GetComponent<PetraAbilityDownCharge>();
        horizontalChargeAbility = GetComponent<PetraAbilityHorizontalCharge>();
        spinAttackAbility = GetComponent<PetraAbilitySpinAttack>();
        chargeAttackAbility = GetComponent<PetraAbilityChargeAttack>();
        basicSlamAbility = GetComponent<PetraAbilityBasicSlam>();
        jumpSlamAbility = GetComponent<PetraAbilityJumpSlam>();
        jumpGroundSlamAbility = GetComponent<PetraAbilityJumpGroundSlam>();
        lookOrientation = GetComponent<LookOrientation>();
        health = GetComponent<Health>();
    }

    protected override void OnActivate()
    {
        health.OnTakeDamage += Health_OnTakeDamage;
        health.OnDeath += Health_OnDeath;

        if (activateCombatBehaviorOnStart)
            ChangeCombatBehavior();
    }

    private void Health_OnTakeDamage()
    {
        if (health.CurrentHP == changePhaseHPThreshold)
        {
            StopCurrentAbility();
            StartCoroutine(ChangePhase());
        }
    }

    private void Health_OnDeath()
    {
        StopCurrentAbility();
    }

    private IEnumerator LoopCombatBehavior(Func<IEnumerator> getAbility)
    {
        IEnumerator ability = getAbility();

        SetFacingDirection();
        yield return StartCoroutine(ability);
        yield return Helper.GetWaitForSeconds(timeVarianceCompensationDelay);

        StartCoroutine(LoopCombatBehavior(getAbility));
    }
    
    private IEnumerator GetFirstPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            return spinAttackAbility.SpinAttack();
        }

        if (IsPlayerInlineHorizontally())
        {
            return horizontalChargeAbility.HorizontalCharge();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? upChargeAbility.UpCharge() : downChargeAbility.DownCharge();
        }

        if (!IsPlayerNearby())
        {
            return basicSlamAbility.BasicSlam();
        }
        
        return null;
    }

    private IEnumerator GetSecondPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            return randomIndex == 0 ? spinAttackAbility.SpinAttack() : chargeAttackAbility.ChargeAttack();
        }

        if (IsPlayerInlineHorizontally())
        {
            return horizontalChargeAbility.HorizontalCharge();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? upChargeAbility.UpCharge() : downChargeAbility.DownCharge();
        }

        if (!IsPlayerNearby())
        {
            int random = GetRandomNumberWithConsecutiveLimit(1, 3, 3);
            return random == 1 ? jumpSlamAbility.JumpSlam() : basicSlamAbility.BasicSlam();
        }

        return null;
    }

    private IEnumerator GetAbilityTesterAbility()
    {
        switch (LoopAbility)
        {
            case Ability.UpCharge:
                return upChargeAbility.UpCharge();
            case Ability.DownCharge:
                return downChargeAbility.DownCharge();
            case Ability.HorizontalCharge:
                return horizontalChargeAbility.HorizontalCharge();
            case Ability.SpinAttack:
                return spinAttackAbility.SpinAttack();
            case Ability.ChargeAttack:
                return chargeAttackAbility.ChargeAttack();
            case Ability.BasicSlam:
                return basicSlamAbility.BasicSlam();
            case Ability.JumpSlam:
                return jumpSlamAbility.JumpSlam();
            default:
                return null;
        }
    }
    
    

    [Button("Change Combat Behavior", ButtonSizes.Medium)]
    private void ChangeCombatBehavior()
    {
        StopCurrentAbility();

        switch (SelectedCombatMode)
        {
            case CombatMode.FirstPhase:
                StartCoroutine(LoopCombatBehavior(GetFirstPhaseAbility));
                break;
            case CombatMode.SecondPhase:
                StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
                break;
            case CombatMode.AbilityLoop:
                StartCoroutine(LoopCombatBehavior(GetAbilityTesterAbility));
                break;
        }
    }

    private IEnumerator ChangePhase()
    {
        yield return jumpGroundSlamAbility.JumpGroundSlam();
        yield return Helper.GetWaitForSeconds(timeVarianceCompensationDelay);
    
        StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
    }

    private void StopCurrentAbility()
    {
        StopAllCoroutines();
        DeactivateAllAttackCollider();
    }

    private int GetRandomNumberWithConsecutiveLimit(int min, int max, int consecutiveLimit)
    {
        int random = UnityEngine.Random.Range(min, max);
        if (random == lastRandomResult)
        {
            consecutiveCount++;
        }
        else
        {
            consecutiveCount = 0;
        }
        if (consecutiveCount > consecutiveLimit)
        {
            while (random == lastRandomResult)
            {
                random = UnityEngine.Random.Range(min, max);
            }
        }
        lastRandomResult = random;
        return random;
    }

    private void DeactivateAllAttackCollider()
    {
        foreach (GameObject collider in attackColliderArray)
        {
            collider.SetActive(false);
        }
    }

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

}
