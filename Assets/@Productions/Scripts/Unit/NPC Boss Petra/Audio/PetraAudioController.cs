using System;
using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using UnityEngine;

public class PetraAudioController : SceneService
{
    [Range(0, 1)]
    [SerializeField] private float basicSlamVolume = 1f;
    [Range(0, 1)]
    [SerializeField] private float chargeAttackVolume = 1f;
    [Range(0, 1)]
    [SerializeField] private float jumpSlamVolume = 1f;
    [Range(0, 1)]
    [SerializeField] private float runChargeVolume = 1f;
    [Range(0, 1)]
    [SerializeField] private float spinAttackVolume = 1f;
    [SerializeField] private AudioClipPetraSO petraAudioSO;

    private PetraCombatBehaviorNew petraCombatBehavior;
    
    private void Awake()
    {
        petraCombatBehavior = GetComponent<PetraCombatBehaviorNew>();
    }

    private void Start()
    {
        petraCombatBehavior.OnBasicSlamUsed += PetraCombatBehavior_OnBasicSlamUsed;
        petraCombatBehavior.OnChargeAttackUsed += PetraCombatBehavior_OnChargeAttackUsed;
        petraCombatBehavior.OnDownChargeUsed += PetraCombatBehavior_OnDownChargeUsed;
        petraCombatBehavior.OnHorizontalChargeUsed += PetraCombatBehavior_OnHorizontalChargeUsed;
        petraCombatBehavior.OnJumpGroundSlamUsed += PetraCombatBehavior_OnJumpGroundSlamUsed;
        petraCombatBehavior.OnJumpSlamUsed += PetraCombatBehavior_OnJumpSlamUsed;
        petraCombatBehavior.OnSpinAttackUsed += PetraCombatBehavior_OnSpinAttackUsed;
        petraCombatBehavior.OnUpChargeUsed += PetraCombatBehavior_OnUpChargeUsed;
    }

    private void PetraCombatBehavior_OnBasicSlamUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.BasicSlam, basicSlamVolume);
    }

    private void PetraCombatBehavior_OnChargeAttackUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.ChargeSlam, chargeAttackVolume);
    }

    private void PetraCombatBehavior_OnDownChargeUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.RunCharge, runChargeVolume);
    }

    private void PetraCombatBehavior_OnHorizontalChargeUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.RunCharge, runChargeVolume);
    }

    private void PetraCombatBehavior_OnJumpGroundSlamUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.JumpSlam, jumpSlamVolume);
    }

    private void PetraCombatBehavior_OnJumpSlamUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.JumpSlam, jumpSlamVolume);
    }

    private void PetraCombatBehavior_OnSpinAttackUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.CoffinSwing, spinAttackVolume);
    }

    private void PetraCombatBehavior_OnUpChargeUsed()
    {
        Context.AudioManager.PlaySoundVolume(petraAudioSO.RunCharge, runChargeVolume);
    }
}
