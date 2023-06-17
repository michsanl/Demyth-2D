using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PetraCombatBehaviorTester : MonoBehaviour
{


    [EnumToggleButtons]
    public CombatBehavior combatBehavior;
    public enum CombatBehavior 
    { AbilityTester, PhaseOne, PhaseTwo }
    
    private PetraAbilityTester abilityTester;
    private PetraPhaseOne phaseOne;
    [ReadOnly]
    public CombatBehavior selectedCombatBehavior;

    private void Awake() 
    {
        abilityTester = GetComponent<PetraAbilityTester>();
        phaseOne = GetComponent<PetraPhaseOne>();
        ChangeBehavior();
    }

    private void Update() 
    {
        if (selectedCombatBehavior != combatBehavior)
        {
            ChangeBehavior();
        }
    }

    private void ChangeBehavior()
    {
        selectedCombatBehavior = combatBehavior;

        switch (combatBehavior)
        {
            case CombatBehavior.AbilityTester:
                abilityTester.EnableAbilityTester = true;
                phaseOne.EnableFirstPhase = false;
                break;
            case CombatBehavior.PhaseOne:
                abilityTester.EnableAbilityTester = false;
                phaseOne.EnableFirstPhase = true;
                break;
            case CombatBehavior.PhaseTwo:
                abilityTester.EnableAbilityTester = false;
                phaseOne.EnableFirstPhase = false;
                break;
            
            default:
                break;
        }
    }


}
