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
        if (player != null)
        {
            if (TryGetComponent<KnockbackBase>(out KnockbackBase knockbackBase))
            {
                if (isKnockbackOnly)
                {
                    player.TriggerKnockBack(knockbackBase.GetKnockbackTargetPosition(player));
                }
                else
                {
                    player.TakeDamage(true, knockbackBase.GetKnockbackTargetPosition(player), damagerCharacter);
                }
            }
            else
            {
                player.TakeDamage(false, Vector2.zero, damagerCharacter);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        player = other.collider.GetComponent<Player>();
    }

}
