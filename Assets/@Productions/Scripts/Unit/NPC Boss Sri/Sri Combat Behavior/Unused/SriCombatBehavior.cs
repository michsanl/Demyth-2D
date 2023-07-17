using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriCombatBehavior : SceneService
{
    [SerializeField] private bool activateCombatMode;
    [EnumToggleButtons] public CombatMode SelectCombatMode;
    [EnumToggleButtons, Space] public Ability LoopAbility;

    public enum Ability
    { UpSlash, DownSlash, HorizontalSlash, SpinClaw, NailAOE, NailSummon, FireBall, HorizontalNailWave, 
    VerticalNailWave, WaveOutNailWave, Teleport }
    public enum CombatMode 
    { FirstPhase, SecondPhase, NewSecondPhase, AbilityLoop }

    private int healthDecreaseCount;
    private bool isPlayingAbility;
    
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

    private Coroutine abilityControllerCoroutine;
    private Coroutine abilityCoroutine;

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
        base.OnActivate();

        health.OnTakeDamage += Health_OnTakeDamage;
    }

    protected override void OnTick()
    {
        if (!activateCombatMode)
            return;
        
        switch (SelectCombatMode)
        {
            case CombatMode.FirstPhase:
                FirstPhaseRoutine();
                break;
            case CombatMode.SecondPhase:
                SecondPhaseRoutine();
                break;
            case CombatMode.NewSecondPhase:
                NewSecondPhaseRoutine();
                break;
            case CombatMode.AbilityLoop:
                AbilityLoopRoutine();
                break;
        }
    }

    [Button("Select Combat Mode", ButtonSizes.Medium)]
    public void ChangeCombatBehavior()
    {
        switch (SelectCombatMode)
        {
            case CombatMode.FirstPhase:
                abilityControllerCoroutine = StartCoroutine(StartCombatBehaviorLoop());
                break;
            case CombatMode.SecondPhase:
                break;
            case CombatMode.NewSecondPhase:
                break;
            case CombatMode.AbilityLoop:
                break;
        }
    }

    private void Health_OnTakeDamage()
    {
        healthDecreaseCount++;
    }

    private void FirstPhaseRoutine()
    {
        if (isPlayingAbility)
            return;

        isPlayingAbility = true;
        SetFacingDirection();

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailAOE());
            } else
            {
                StartCoroutine(PlayAbilitySpinClaw());
            }
            return;
        }


        if (IsPlayerInlineHorizontally())
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        }
        if (IsPlayerInlineVertically())
        {
            PlayVerticalAbility();
            return;
        }


        if (!IsPlayerNearby())
        {
            StartCoroutine(PlayAbilityNailSummon());
        }
        
    }

    private IEnumerator StartCombatBehaviorLoop()
    {
        yield return abilityCoroutine = StartCoroutine(FirstPhaseAbilityRoutine());
        abilityControllerCoroutine = StartCoroutine(StartCombatBehaviorLoop());
    }
    
    private IEnumerator FirstPhaseAbilityRoutine()
    {
        SetFacingDirection();

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0,3);
            if (randomIndex == 0)
            {
                yield return abilityCoroutine = StartCoroutine(nailAOEAbility.NailAOE());
            } else
            {
                yield return abilityCoroutine = StartCoroutine(spinClawAbility.SpinClaw());
            }
            yield break;
        }


        if (IsPlayerInlineHorizontally())
        {
            yield return abilityCoroutine = StartCoroutine(horizontalSlashAbility.HorizontalSlash());
            yield break;
        }


        if (IsPlayerInlineVertically())
        {
            if (IsPlayerAbove())
            {
                yield return abilityCoroutine = StartCoroutine(upSlashAbility.UpSlash());
            }
            else
            {
                yield return abilityCoroutine = StartCoroutine(downSlashAbility.DownSlash());
            }
            yield break;
        }


        if (!IsPlayerNearby())
        {
            yield return abilityCoroutine = StartCoroutine(nailSummonAbility.NailSummon());
        }
        
    }

    private void SecondPhaseRoutine()
    {
        if (isPlayingAbility)
            return;
            
        SetFacingDirection();

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            StartCoroutine(PlayAbilityTeleport());
            SetFacingDirection();
            return;
        }

        if (IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailAOE());
            }
            else
            {
                StartCoroutine(PlayAbilitySpinClaw());
            }
            return;
        }

        if (IsPlayerInlineHorizontally())
        {
            StartCoroutine(PlayAbilityHorizontalSlash());
            return;
        }
        if (IsPlayerInlineVertically())
        {
            PlayVerticalAbility();
            return;
        }


        if (!IsPlayerNearby())
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0)
            {
                StartCoroutine(PlayAbilityNailSummon());
            }
            else
            {
                StartCoroutine(PlayAbilityFireBall());
            }
        }
    }

    private void NewSecondPhaseRoutine()
    {
        if (healthDecreaseCount >= 3)
        {
            StopCurrentAbility();

            SetFacingDirection();
            abilityControllerCoroutine = StartCoroutine(PlayAbilityWaveOutNailWave());

            healthDecreaseCount = 0;
            return;
        }
        if (isPlayingAbility)
            return;

        SetFacingDirection();

        healthDecreaseCount = 0;
        abilityControllerCoroutine = StartCoroutine(TeleportIntoNailWaveVariant());
    }

    private void AbilityLoopRoutine()
    {
        if (isPlayingAbility)
            return;
        
        SetFacingDirection();

        switch (LoopAbility)
        {
            case Ability.UpSlash:
                StartCoroutine(PlayAbility(upSlashAbility.UpSlash()));
                break;
            case Ability.DownSlash:
                StartCoroutine(PlayAbility(downSlashAbility.DownSlash()));
                break;
            case Ability.HorizontalSlash:
                StartCoroutine(PlayAbility(horizontalSlashAbility.HorizontalSlash()));
                break;
            case Ability.SpinClaw:
                StartCoroutine(PlayAbility(spinClawAbility.SpinClaw()));
                break;
            case Ability.NailAOE:
                StartCoroutine(PlayAbility(nailAOEAbility.NailAOE()));
                break;
            case Ability.NailSummon:
                StartCoroutine(PlayAbility(nailSummonAbility.NailSummon()));
                break;
            case Ability.FireBall:
                StartCoroutine(PlayAbility(fireBallAbility.FireBall()));
                break;
            case Ability.HorizontalNailWave:
                StartCoroutine(PlayAbility(horizontalNailWaveAbility.HorizontalNailWave()));
                break;
            case Ability.VerticalNailWave:
                StartCoroutine(PlayAbility(verticalNailWaveAbility.VerticalNailWave()));
                break;
            case Ability.WaveOutNailWave:
                StartCoroutine(PlayAbility(waveOutNailWaveAbility.WaveOutNailWave()));
                break;
            case Ability.Teleport:
                StartCoroutine(PlayAbility(teleportAbility.Teleport()));
                break;
            default:
                break;
        }
    }
    
    private void StopCurrentAbility()
    {
        if (abilityControllerCoroutine != null)
            StopCoroutine(abilityControllerCoroutine);
        if (abilityCoroutine != null)
            StopCoroutine(abilityCoroutine);
    }

    private IEnumerator TeleportIntoNailWaveVariant()
    {
        isPlayingAbility = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return abilityCoroutine = StartCoroutine(teleportAbility.Teleport());            
            SetFacingDirection();
        }

        int randomNumber = UnityEngine.Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                yield return abilityCoroutine = StartCoroutine(horizontalNailWaveAbility.HorizontalNailWave());
                break;
            case 1:
                yield return abilityCoroutine = StartCoroutine(verticalNailWaveAbility.VerticalNailWave());
                break;
            case 2:
                yield return abilityCoroutine = StartCoroutine(waveOutNailWaveAbility.WaveOutNailWave());
                break;
            default:
                break;
        }

        isPlayingAbility = false;
    }

    private void PlayVerticalAbility()
    {
        if (IsPlayerAbove())
        {
            StartCoroutine(PlayAbilityUpSlash());
            return;
        } else
        {
            StartCoroutine(PlayAbilityDownSlash());
            return;
        }
    }

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

    protected IEnumerator PlayAbilityUpSlash()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(upSlashAbility.UpSlash());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityDownSlash()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(downSlashAbility.DownSlash());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityHorizontalSlash()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(horizontalSlashAbility.HorizontalSlash());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilitySpinClaw()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(spinClawAbility.SpinClaw());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityNailAOE()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(nailAOEAbility.NailAOE());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityNailSummon()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(nailSummonAbility.NailSummon());
        isPlayingAbility = false;
    }
    
    protected IEnumerator PlayAbilityFireBall()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(fireBallAbility.FireBall());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityHorizontalNailWave()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(horizontalNailWaveAbility.HorizontalNailWave());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityVerticalNailWave()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(verticalNailWaveAbility.VerticalNailWave());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbilityWaveOutNailWave()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(waveOutNailWaveAbility.WaveOutNailWave());
        isPlayingAbility = false;
    }
    
    protected IEnumerator PlayAbilityTeleport()
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(teleportAbility.Teleport());
        isPlayingAbility = false;
    }

    protected IEnumerator PlayAbility(IEnumerator ability)
    {
        isPlayingAbility = true;
        yield return abilityCoroutine = StartCoroutine(ability);
        isPlayingAbility = false;
    }

    protected IEnumerator TeleportMultipleTimes()
    {
        isPlayingAbility = true;

        int teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(teleportAbility.Teleport());            
            SetFacingDirection();
        }

        isPlayingAbility = false;
    }
}
