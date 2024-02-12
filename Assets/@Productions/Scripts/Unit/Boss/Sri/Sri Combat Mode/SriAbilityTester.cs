using System.Collections;
using System.Collections.Generic;

public class SriAbilityTester : AbilitySelector
{
    
    private SriBossController _sriBossController;
    private SriAbilityController _abilityContainer;
    
    private void Awake()
    {
        _sriBossController = GetComponent<SriBossController>();
        _abilityContainer = GetComponent<SriAbilityController>();
    }

    public override IEnumerator GetAbility()
    {
        switch (_sriBossController.SelectedAbility)
        {
            case SriBossController.Ability.UpSlash:
                return _abilityContainer.StartUpSlashAbility();
            case SriBossController.Ability.DownSlash:
                return _abilityContainer.StartDownSlashAbility();
            case SriBossController.Ability.HorizontalSlash:
                return _abilityContainer.StartHorizontalSlashAbility();
            case SriBossController.Ability.SpinClaw:
                return _abilityContainer.StartSpinClawAbility();
            case SriBossController.Ability.NailAOE:
                return _abilityContainer.StartNailAOEAbility();
            case SriBossController.Ability.NailSummon:
                return _abilityContainer.StartNailSummonAbility();
            case SriBossController.Ability.FireBall:
                return _abilityContainer.StartFireBallAbility();
            case SriBossController.Ability.HorizontalNailWave:
                return _abilityContainer.StartHorizontalNailWaveAbility();
            case SriBossController.Ability.VerticalNailWave:
                return _abilityContainer.StartVerticalNailWaveAbility();
            case SriBossController.Ability.WaveOutNailWave:
                return _abilityContainer.StartWaveOutNailWaveAbility();
            case SriBossController.Ability.Teleport:
                return _abilityContainer.StartTeleportAbility();
            case SriBossController.Ability.DeathSlash:
                return _abilityContainer.StartDeathSlashAbility();
            default:
                return null;
        }
    }
}
