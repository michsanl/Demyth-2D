using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using CustomTools.Core;

public class PlayerDamager : SceneService
{

    [SerializeField] public DamagerCharacter damagerCharacter;
    [SerializeField] private bool isKnockbackOnly;

    public enum DamagerCharacter { NotSet, Petra, Sri }

    private Player player;

    private void OnCollisionStay(Collision other) 
    {
        if (other == null)
            return;
        if (player == null)
            return;
            
        if (TryGetComponent<KnockbackBase>(out KnockbackBase knockbackBase))
        {
            if (isKnockbackOnly)
            {
                player.ApplyKnockBackToPlayer(knockbackBase.GetKnockbackTargetPosition(player));
            }
            else
            {
                player.ApplyDamageToPlayer(true, knockbackBase.GetKnockbackTargetPosition(player));
            }
        }
        else
        {
            player.ApplyDamageToPlayer(false, Vector2.zero);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        player = other.collider.GetComponent<Player>();
    }

}
