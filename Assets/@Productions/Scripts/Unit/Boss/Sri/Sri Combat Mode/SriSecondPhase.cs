using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Demyth.Gameplay;

public class SriSecondPhase : AbilitySelector
{

    private Player _player;
    private SriAbilityController _abilityContainer;
    private int _rangeAbilityCount;
    private int _meleeAbilityCounter;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _abilityContainer = GetComponent<SriAbilityController>();
    }
    
    public override IEnumerator GetAbility()
    {
        if (Random.Range(0, 3) == 0)
        {
            return _abilityContainer.StartTeleportAbility();
        }

        if (IsPlayerNearby())
        {
            _rangeAbilityCount = 0;
            _meleeAbilityCounter++;
            if (_meleeAbilityCounter > 2)
            {
                _meleeAbilityCounter = 0;
            }
            return _meleeAbilityCounter == 0 ? _abilityContainer.StartSpinClawAbility() : _abilityContainer.StartNailAOEAbility();
        }

        if (IsPlayerInlineHorizontally())
        {
            _rangeAbilityCount = 0;
            return _abilityContainer.StartHorizontalSlashAbility();
        }

        if (IsPlayerInlineVertically())
        {
            _rangeAbilityCount = 0;
            return IsPlayerAbove() ? _abilityContainer.StartUpSlashAbility() : _abilityContainer.StartDownSlashAbility();
        }

        if (!IsPlayerNearby())
        {
            _rangeAbilityCount++;
            if (_rangeAbilityCount > 4)
            {
                _rangeAbilityCount = 0;
                return _abilityContainer.StartTeleportAbility();
            }

            int randomIndex = Random.Range(0, 2);
            return randomIndex == 0 ? _abilityContainer.StartFireBallAbility() : _abilityContainer.StartNailSummonAbility();
        }

        return null;
    }

    protected bool IsPlayerAbove()
    {
        return transform.position.y < _player.transform.position.y;
    }

    protected bool IsPlayerInlineVertically()
    {
        return Mathf.Approximately(transform.position.x, _player.transform.position.x) ;
    }

    protected bool IsPlayerInlineHorizontally()
    {
        return Mathf.Approximately(transform.position.y, _player.transform.position.y);
    }

    protected bool IsPlayerNearby()
    {
        return Vector2.Distance(transform.position, _player.transform.position) < 1.5f;
    }
}
