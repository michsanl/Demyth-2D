using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityChargeAttack : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private GameObject groundCoffinAOE;
    
    private int CHARGE_ATTACK = Animator.StringToHash("Charge_attack");
    
    public IEnumerator ChargeAttack(Animator animator)
    {
        animator.Play(CHARGE_ATTACK);
        // audioManager.PlaySound(audioManager.PetraAudioSource.ChargeSlam);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        Instantiate(groundCoffinAOE, transform.position, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(swingDuration);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
