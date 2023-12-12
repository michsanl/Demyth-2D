using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityVerticalNailWave : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject verticalNailWave;
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator VerticalNailWave(Animator animator, AudioClip abilitySFX)
    {
        animator.Play(NAIL_WAVE);
        PlayAudio(abilitySFX);

        LeanPool.Spawn(verticalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
