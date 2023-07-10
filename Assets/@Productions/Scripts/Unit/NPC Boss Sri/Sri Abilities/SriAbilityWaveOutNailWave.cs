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
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator PlayAbility()
    {
        var audioManager = Context.AudioManager;

        animator.Play(NAIL_WAVE);
        // audioManager.PlayClipAtPoint(audioManager.SriAudioSource.NailSummon, transform.position);
        StartCoroutine(SpawnNail());

        yield return Helper.GetWaitForSeconds(animationDuration);
    }

    private IEnumerator SpawnNail()
    {
        yield return Helper.GetWaitForSeconds(nailSpawnDelay);
        
        Instantiate(waveOutNailWave, new Vector3(0, -1, 0), Quaternion.identity);
    }
}
