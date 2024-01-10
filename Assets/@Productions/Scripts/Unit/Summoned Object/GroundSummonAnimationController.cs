using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSummonAnimationController : MonoBehaviour
{
    private int GROUND_SUMMON_IN = Animator.StringToHash("Ground_Summon_In");
    private int GROUND_SUMMON_ATTACK = Animator.StringToHash("Ground_Summon_Attack");
    private int GROUND_SUMMON_OUT = Animator.StringToHash("Ground_Summon_Out");
    private Animator animator;
    private GroundSummon groundSummon;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        groundSummon = GetComponentInParent<GroundSummon>();

        groundSummon.OnAnticipation += GroundSummon_OnAnticipation;
        groundSummon.OnAttack += GroundSummon_OnAttack;
        groundSummon.OnRecovery += GroundSummon_OnRecovery;
    }

    private void GroundSummon_OnAnticipation()
    {
        animator.Play(GROUND_SUMMON_IN);
    }

    private void GroundSummon_OnAttack()
    {
        animator.Play(GROUND_SUMMON_ATTACK);
    }

    private void GroundSummon_OnRecovery()
    {
        animator.Play(GROUND_SUMMON_OUT);
    }
}
