using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityHorizontalNailWave : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _introProp;
    [SerializeField] private GameObject horizontalNailWave;
    [SerializeField] private SriClipSO _sriClipSO;

    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator HorizontalNailWave(Animator animator)
    {
        animator.SetFloat("Nail_AOE_Multiplier", _introProp.AnimationSpeedMultiplier);
        
        animator.SetTrigger(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        LeanPool.Spawn(horizontalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_introProp.GetSwingDuration());
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
