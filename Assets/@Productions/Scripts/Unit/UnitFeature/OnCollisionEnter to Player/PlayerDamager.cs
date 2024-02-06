using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{

    [SerializeField] private bool isKnockbackOnly;
    [SerializeField] private BossClipSO _bossClipSO;

    private Player player;

    private void OnCollisionEnter(Collision other)
    {
        player = other.collider.GetComponent<Player>();
    }

    private void OnCollisionStay(Collision other) 
    {
        if (other == null)
            return;
        if (player == null)
            return;
            
        if (TryGetComponent(out KnockbackBase knockbackBase))
        {
            if (isKnockbackOnly)
            {
                KnockbackPlayer(knockbackBase);
            }
            else
            {
                TryDamagePlayer(true, knockbackBase.GetKnockbackTargetPosition(player));
            }
        }
        else
        {
            TryDamagePlayer(false);
        }
    }

    private void KnockbackPlayer(KnockbackBase knockbackBase)
    {
        player.ApplyKnockBackToPlayer(knockbackBase.GetKnockbackTargetPosition(player));
        PlayRandomBossDamageSFX();
    }

    private void TryDamagePlayer(bool enableKnockback, Vector2 knockbackTargetPosition = default)
    {
        var canDamagePlayer = player.ApplyDamageToPlayer(enableKnockback, knockbackTargetPosition);
        if (canDamagePlayer)
        {
            PlayRandomBossDamageSFX();
        }
    }

    private void PlayRandomBossDamageSFX()
    {
        if (_bossClipSO != null)
        {
            var randomIndex = Random.Range(0, _bossClipSO.GetDamageAudioLength());
            Helper.PlaySFX(_bossClipSO.GetDamageAudioClip(randomIndex), _bossClipSO.GetDamageVolume(randomIndex));
        }
    }
}
