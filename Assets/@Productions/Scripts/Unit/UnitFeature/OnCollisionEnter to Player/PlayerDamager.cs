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
    [SerializeField] private PetraClipSO _petraClipSO;
    [SerializeField] private SriClipSO _sriClipSO;

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
                TryDamagePlayer(true, knockbackBase.GetKnockbackTargetPosition(player));
            }
        }
        else
        {
            TryDamagePlayer(false, Vector2.zero);
        }
    }

    private void TryDamagePlayer(bool enableKnockback, Vector2 knockbackTargetPosition)
    {
        bool canDamagePlayer = player.ApplyDamageToPlayer(enableKnockback, knockbackTargetPosition);
        int random;

        if (canDamagePlayer)
        {
            if (_petraClipSO != null)
            {
                random = Random.Range(0, _petraClipSO.Damage.Length);
                Helper.PlaySFX(_petraClipSO.Damage[random], _petraClipSO.GetDamageVolume(random));
            }
            if (_sriClipSO != null)
            {
                random = Random.Range(0, _sriClipSO.Damage.Length);
                Helper.PlaySFX(_sriClipSO.Damage[random], _sriClipSO.GetDamageVolume(random));
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        player = other.collider.GetComponent<Player>();
    }

}
