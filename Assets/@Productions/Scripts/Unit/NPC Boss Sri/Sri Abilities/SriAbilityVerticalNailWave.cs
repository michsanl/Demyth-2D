using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityVerticalNailWave : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject verticalNailWave;
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator VerticalNailWave()
    {
        animator.Play(NAIL_WAVE);
        var audioManager = Context.AudioManager;
        audioManager.PlaySound(audioManager.SriAudioSO.NailAOE);
        Instantiate(verticalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuration);
    }
}
