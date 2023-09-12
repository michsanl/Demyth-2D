using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityHorizontalNailWave : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject horizontalNailWave;

    private GameObject spawnedNailGO;
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator HorizontalNailWave()
    {
        var audioManager = Context.AudioManager;

        animator.Play(NAIL_WAVE);
        audioManager.PlaySound(audioManager.SriAudioSO.NailAOE);
        spawnedNailGO = Instantiate(horizontalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuration);
    }

    public GameObject GetNailGameObject()
    {
        return spawnedNailGO;
    }
}
