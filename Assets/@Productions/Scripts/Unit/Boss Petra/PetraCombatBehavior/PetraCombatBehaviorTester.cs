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
    private PetraPhaseTwo phaseTwo;
    private CombatBehavior selectedCombatBehavior;

    private void Awake() 
    {
        abilityTester = GetComponent<PetraAbilityTester>();
        phaseOne = GetComponent<PetraPhaseOne>();
        phaseTwo = GetComponent<PetraPhaseTwo>();
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
                phaseTwo.EnableSecondPhase = false;
                break;
            case CombatBehavior.PhaseOne:
                abilityTester.EnableAbilityTester = false;
                phaseOne.EnableFirstPhase = true;
                phaseTwo.EnableSecondPhase = false;
                break;
            case CombatBehavior.PhaseTwo:
                abilityTester.EnableAbilityTester = false;
                phaseOne.EnableFirstPhase = false;
                phaseTwo.EnableSecondPhase = true;
                break;
            
            default:
                break;
        }
    }


}
