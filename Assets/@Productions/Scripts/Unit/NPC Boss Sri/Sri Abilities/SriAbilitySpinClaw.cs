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
    [SerializeField] private AnimationPropertiesSO _spinClawProp;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spinClawCollider;
    [SerializeField] private SriClipSO _sriClipSO;
    
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");

    public IEnumerator SpinClaw(Animator animator)
    {
        animator.SetFloat("Spin_Claw_Multiplier", _spinClawProp.AnimationSpeedMultiplier);
        
        animator.SetTrigger(SPIN_CLAW);
        Helper.PlaySFX(_sriClipSO.SpinClaw, _sriClipSO.SpinClawVolume);

        yield return Helper.GetWaitForSeconds(_spinClawProp.GetFrontSwingDuration());
        spinClawCollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(_spinClawProp.GetSwingDuration());
        spinClawCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_spinClawProp.GetBackSwingDuration());
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
