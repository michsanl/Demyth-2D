using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilitySpinClaw : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spinClawCollider;
    
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");

    public IEnumerator SpinClaw(Animator animator, AudioClip abilitySFX)
    {
        animator.Play(SPIN_CLAW);
        PlayAudio(abilitySFX);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        spinClawCollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swingDuration);
        spinClawCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
