using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class SriThirdPhase : AbilitySelector
{

    private Player _player;
    private SriAbilityController _abilityContainer;
    private LookOrientation _lookOrientation;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _abilityContainer = GetComponent<SriAbilityController>();
        _lookOrientation= GetComponent<LookOrientation>();
    }

    public override IEnumerator GetAbility()
    {
        return ThirdPhaseAbility();
    }

    private IEnumerator ThirdPhaseAbility()
    {
        var teleportCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < teleportCount; i++)
        {
            yield return StartCoroutine(_abilityContainer.StartTeleportAbility());            
            SetFacingDirectionToPlayer();
        }

        var rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            yield return StartCoroutine(_abilityContainer.StartHorizontalNailWaveAbility());
        }
        else
        {
            yield return StartCoroutine(_abilityContainer.StartVerticalNailWaveAbility());
        }
    }

    protected void SetFacingDirectionToPlayer()
    {
        if (IsPlayerToRight())
        {
            _lookOrientation.SetFacingDirection(Vector2.right);
        }

        if (IsPlayerToLeft())
        {
            _lookOrientation.SetFacingDirection(Vector2.left);
        }
    }

    protected bool IsPlayerToRight()
    {
        return transform.position.x < _player.transform.position.x;
    }

    protected bool IsPlayerToLeft()
    {
        return transform.position.x > _player.transform.position.x;
    }
}
