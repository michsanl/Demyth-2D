using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ability Timeline/New Ability Timeline")]
public class AbilityTimelineSO : ScriptableObject
{
    [OnValueChanged("CalculateAnticipation")]
    public float AnticipationDuration;
    [OnValueChanged("CalculateAttack")]
    public float AttackDuration;
    [OnValueChanged("CalculateRecovery")]
    public float RecoveryDuration;

    [Space]

    [InfoBox("Change applied after restarting the game")]
    [Range(0f, 5f), OnValueChanged("CalculateAnticipation")]
    public float AnticipationMultiplier = 1f;
    [Range(0f, 5f), OnValueChanged("CalculateAttack")]
    public float AttackMultiplier = 1f;
    [Range(0f, 5f), OnValueChanged("CalculateRecovery")]
    public float RecoveryMultiplier = 1f;

    [Space]

    [ReadOnly]
    public float FinalAnticiptionDuration;
    [ReadOnly]
    public float FinalAttackDuration;
    [ReadOnly]
    public float FinalRecoveryDuration;

    private void CalculateAnticipation()
    {
        FinalAnticiptionDuration = AnticipationDuration / AnticipationMultiplier;
    }
    private void CalculateAttack()
    {
        FinalAttackDuration = AttackDuration / AttackMultiplier;
    }
    private void CalculateRecovery()
    {
        FinalRecoveryDuration = RecoveryDuration / RecoveryMultiplier;
    }
}
