using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetraAnimatorController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    
    private int BASIC_SLAM = Animator.StringToHash("Basic_Slam");
    private int CHARGE_ATTACK = Animator.StringToHash("Charge_attack");
    private int DOWN_CHARGE = Animator.StringToHash("Down_charge");
    private int HORIZONTAL_CHARGE = Animator.StringToHash("Side_charge");
    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");
    private int SPIN_ATTACK = Animator.StringToHash("Spin_attack");
    private int UP_CHARGE = Animator.StringToHash("Up_charge");

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
        animator.Play(BASIC_SLAM);
    }

    private void PetraCombatBehavior_OnChargeAttackUsed()
    {
        animator.Play(CHARGE_ATTACK);
    }

    private void PetraCombatBehavior_OnDownChargeUsed()
    {
        animator.Play(DOWN_CHARGE);
    }

    private void PetraCombatBehavior_OnHorizontalChargeUsed()
    {
        animator.Play(HORIZONTAL_CHARGE);
    }

    private void PetraCombatBehavior_OnJumpGroundSlamUsed()
    {
        animator.Play(JUMP_SLAM);
    }

    private void PetraCombatBehavior_OnJumpSlamUsed()
    {
        animator.Play(JUMP_SLAM);
    }

    private void PetraCombatBehavior_OnSpinAttackUsed()
    {
        animator.Play(SPIN_ATTACK);
    }

    private void PetraCombatBehavior_OnUpChargeUsed()
    {
        animator.Play(UP_CHARGE);
    }
}
