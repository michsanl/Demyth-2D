using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilityWaveOutNailWave : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject waveOutNailWave;
    
    private float teleportStartDuration = 0.3f;
    private float teleportEndDuration = 0.4f;
    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator WaveOutNailWave(Animator animator, AudioClip abilitySFX)
    {
        yield return StartCoroutine(TeleportToMiddleArena());

        animator.Play(NAIL_WAVE);
        PlayAudio(abilitySFX);

        StartCoroutine(SpawnNail());

        yield return Helper.GetWaitForSeconds(animationDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    public IEnumerator TeleportToMiddleArena()
    {
        animator.Play(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(teleportStartDuration);

        transform.position = new Vector3(0, -1, 0);

        animator.Play(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(teleportEndDuration);
    }

    private IEnumerator SpawnNail()
    {
        yield return Helper.GetWaitForSeconds(nailSpawnDelay);

        Instantiate(waveOutNailWave, new Vector3(0, -1, 0), Quaternion.identity);
    }
}
