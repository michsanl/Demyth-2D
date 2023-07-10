using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityWaveOutNailWave : SceneService
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

    public IEnumerator PlayAbility()
    {
        var audioManager = Context.AudioManager;

        yield return StartCoroutine(TeleportToMiddleArena());

        animator.Play(NAIL_WAVE);
        // audioManager.PlayClipAtPoint(audioManager.SriAudioSource.NailAOE, transform.position);
        StartCoroutine(SpawnNail());

        yield return Helper.GetWaitForSeconds(animationDuration);
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
