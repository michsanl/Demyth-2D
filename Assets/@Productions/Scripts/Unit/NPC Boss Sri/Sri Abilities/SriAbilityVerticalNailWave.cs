using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityVerticalNailWave : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuratoin;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject verticalNailWave;
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator PlayAbility()
    {
        var audioManager = Context.AudioManager;

        animator.Play(NAIL_WAVE);
        // audioManager.PlayClipAtPoint(audioManager.SriAudioSource.NailSummon, transform.position);
        Instantiate(verticalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuratoin);
    }
}
