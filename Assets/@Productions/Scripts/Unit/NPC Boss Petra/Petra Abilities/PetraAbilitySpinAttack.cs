using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class PetraAbilitySpinAttack : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float _frontSwingDuration = 0.45f;
    [SerializeField] private float _swingDuration = 0.55f;
    [SerializeField] private float _backSwingDuration = 0.75f;
    
    [Title("Components")]
    [SerializeField] private GameObject spinAttackCollider;
    
    private int SPIN_ATTACK = Animator.StringToHash("Spin_attack");
    
    public IEnumerator SpinAttack(Animator animator, AudioClip abilitySFX)
    {
        animator.Play(SPIN_ATTACK);
        PlayAudio(abilitySFX);

        yield return Helper.GetWaitForSeconds(_frontSwingDuration);
        spinAttackCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(_swingDuration);
        spinAttackCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_backSwingDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
