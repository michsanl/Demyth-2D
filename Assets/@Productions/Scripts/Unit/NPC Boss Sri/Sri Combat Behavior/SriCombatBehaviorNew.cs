using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriCombatBehaviorNew : SceneService
{
    [SerializeField] private bool activateCombatBehaviorOnStart;
    [SerializeField] private int changePhaseHPThreshold;
    [EnumToggleButtons] public CombatMode SelectCombatMode;
    [EnumToggleButtons, Space] public Ability LoopAbility;
    [SerializeField] private GameObject[] attackColliderArray;

    public enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport }
    public enum CombatMode 
    { FirstPhase, SecondPhase, NewSecondPhase, AbilityLoop }

    private SriAbilityUpSlash upSlashAbility;
    private SriAbilityDownSlash downSlashAbility;
    private SriAbilityHorizontalSlash horizontalSlashAbility;
    private SriAbilitySpinClaw spinClawAbility;
    private SriAbilityNailAOE nailAOEAbility;
    private SriAbilityNailSummon nailSummonAbility;
    private SriAbilityFireBall fireBallAbility;
    private SriAbilityTeleport teleportAbility;
    private SriAbilityHorizontalNailWave horizontalNailWaveAbility;
    private SriAbilityVerticalNailWave verticalNailWaveAbility;
    private SriAbilityWaveOutNailWave waveOutNailWaveAbility;
    private LookOrientation lookOrientation;
    private Health health;

    private int meleeAbilityCounter;

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
            StartPhaseTwo();
        }
    }

    private void Health_OnDeath()
    {
        StopCurrentAbility();
    }

    [Button("Change Combat Behavior", ButtonSizes.Medium)]
    private void ChangeCombatBehavior()
    {
        StopCurrentAbility();

        switch (SelectCombatMode)
        {
            case CombatMode.FirstPhase:
                StartCoroutine(LoopCombatBehavior(GetFirstPhaseAbility));
                break;
            case CombatMode.SecondPhase:
                StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
                break;
            case CombatMode.NewSecondPhase:
                StartCoroutine(LoopCombatBehavior(GetNewSecondPhaseAbility));
                break;
            case CombatMode.AbilityLoop:
                StartCoroutine(LoopCombatBehavior(GetAbilityTesterAbility));
                break;
        }
    }

    private IEnumerator LoopCombatBehavior(Func<IEnumerator> getAbility)
    {
        IEnumerator ability = getAbility();
        SetFacingDirection();
        yield return StartCoroutine(ability);
        StartCoroutine(LoopCombatBehavior(getAbility));
    }

    private void IncreaseMeleeAbilityCounter()
    {
        meleeAbilityCounter++;
        if (meleeAbilityCounter > 2)
        {
            meleeAbilityCounter = 0;
        }
    }
    
    private IEnumerator GetFirstPhaseAbility()
    {
        if (IsPlayerNearby())
        {
            IncreaseMeleeAbilityCounter();
            return meleeAbilityCounter == 0 ? nailAOEAbility.NailAOE() : spinClawAbility.SpinClaw();
        }

        if (IsPlayerInlineHorizontally())
        {
            return horizontalSlashAbility.HorizontalSlash();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? upSlashAbility.UpSlash() : downSlashAbility.DownSlash();
        }

        if (!IsPlayerNearby())
        {
            return nailSummonAbility.NailSummon();
        }

        return null;
    }

    private IEnumerator GetSecondPhaseAbility()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return teleportAbility.Teleport();
        }

        if (IsPlayerNearby())
        {
            IncreaseMeleeAbilityCounter();
            return meleeAbilityCounter == 0 ? spinClawAbility.SpinClaw() : nailAOEAbility.NailAOE();
        }

        if (IsPlayerInlineHorizontally())
        {
            return horizontalSlashAbility.HorizontalSlash();
        }

        if (IsPlayerInlineVertically())
        {
            return IsPlayerAbove() ? upSlashAbility.UpSlash() : downSlashAbility.DownSlash();
        }

        if (!IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            return randomIndex == 0 ? fireBallAbility.FireBall() : nailSummonAbility.NailSummon();
        }

        return null;
    }

    private IEnumerator GetNewSecondPhaseAbility()
    {
        return TeleportIntoNailWaveVariant();
    }

    private IEnumerator GetAbilityTesterAbility()
    {
        switch (LoopAbility)
        {
            case Ability.UpSlash:
                return upSlashAbility.UpSlash();
            case Ability.DownSlash:
                return downSlashAbility.DownSlash();
            case Ability.HorizontalSlash:
                return horizontalSlashAbility.HorizontalSlash();
            case Ability.SpinClaw:
                return spinClawAbility.SpinClaw();
            case Ability.NailAOE:
                return nailAOEAbility.NailAOE();
            case Ability.NailSummon:
                return nailSummonAbility.NailSummon();
            case Ability.FireBall:
                return fireBallAbility.FireBall();
            case Ability.HorizontalNailWave:
                return horizontalNailWaveAbility.HorizontalNailWave();
            case Ability.VerticalNailWave:
                return verticalNailWaveAbility.VerticalNailWave();
            case Ability.WaveOutNailWave:
                return waveOutNailWaveAbility.WaveOutNailWave();
            case Ability.Teleport:
                return teleportAbility.Teleport();
            default:
                return null;
        }
    }

    private IEnumerator TeleportIntoNailWaveVariant()
    {
        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(teleportAbility.Teleport());            
            SetFacingDirection();
        }

        int randomNumber = UnityEngine.Random.Range(0, 2);
        switch (randomNumber)
        {
            case 0:
                yield return StartCoroutine(horizontalNailWaveAbility.HorizontalNailWave());
                break;
            case 1:
                yield return StartCoroutine(verticalNailWaveAbility.VerticalNailWave());
                break;
            default:
                break;
        }
    }

    private void StartPhaseTwo()
    {
        StartCoroutine(LoopCombatBehavior(GetSecondPhaseAbility));
    }

    private void StopCurrentAbility()
    {
        StopAllCoroutines();
        DeactivateAllAttackCollider();
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

#region Position to Player Checker

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
