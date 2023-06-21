using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SriAbilityNailAOE : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailAOECollider;
    [SerializeField] private GameObject nailProjectile;
    
    protected int NAIL_AOE = Animator.StringToHash("Nail_AOE");

    public IEnumerator NailAOE(bool summonProjectile)
    {
        if (summonProjectile)
            Instantiate(nailProjectile, transform.position, Quaternion.identity);

        animator.Play(NAIL_AOE);
        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailAOECollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swingDuration);
        nailAOECollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
