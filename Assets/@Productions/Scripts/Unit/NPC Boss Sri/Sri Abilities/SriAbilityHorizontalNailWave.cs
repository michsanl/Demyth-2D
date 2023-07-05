using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityHorizontalNailWave : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject horizontalNailWave;
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator HorizontalNailWaveSummon()
    {
        var audioManager = Context.AudioManager;

        animator.Play(NAIL_WAVE);
        audioManager.PlayClipAtPoint(audioManager.SriAudioSource.NailSummon, transform.position);
        Instantiate(horizontalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);

        yield return Helper.GetWaitForSeconds(swingDuration);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
