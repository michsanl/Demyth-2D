using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilityHorizontalNailWave : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private GameObject horizontalNailWave;

    private GameObject spawnedNailGO;
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator HorizontalNailWave(Animator animator, AudioClip abilitySFX)
    {

        animator.Play(NAIL_WAVE);
        PlayAudio(abilitySFX);
        spawnedNailGO = Instantiate(horizontalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    public GameObject GetNailGameObject()
    {
        return spawnedNailGO;
    }
}
