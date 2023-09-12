using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilitySpinAttack : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spinAttackCollider;
    
    private int SPIN_ATTACK = Animator.StringToHash("Spin_attack");
    
    public IEnumerator SpinAttack()
    {
        animator.Play(SPIN_ATTACK);
        var audioManager = Context.AudioManager;
        audioManager.PlaySound(audioManager.PetraAudioSource.CoffinSwing);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        spinAttackCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);
        spinAttackCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
