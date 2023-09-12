using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilitySpinClaw : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spinClawCollider;
    
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");

    public IEnumerator SpinClaw()
    {
        var audioManager = Context.AudioManager;

        animator.Play(SPIN_CLAW);
        audioManager.PlaySound(audioManager.SriAudioSO.SpinClaw);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        spinClawCollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swingDuration);
        spinClawCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
