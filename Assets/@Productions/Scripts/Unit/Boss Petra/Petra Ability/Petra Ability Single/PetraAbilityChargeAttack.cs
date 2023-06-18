using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityChargeAttack : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject chargeAttackCollider;
    [SerializeField] private GameObject groundCoffinAOE;
    
    private int CHARGE_ATTACK = Animator.StringToHash("Charge_attack");
    
    public IEnumerator ChargeAttack()
    {
        animator.Play(CHARGE_ATTACK);
        // audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        chargeAttackCollider.SetActive(true);
        Instantiate(groundCoffinAOE, transform.position, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(swingDuration);
        chargeAttackCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
