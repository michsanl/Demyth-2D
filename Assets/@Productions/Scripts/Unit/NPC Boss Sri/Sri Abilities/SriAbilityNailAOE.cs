using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

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
    [SerializeField] private SriClipSO _sriClipSO;
    
    protected int NAIL_AOE = Animator.StringToHash("Nail_AOE");

    public IEnumerator NailAOE(Animator animator)
    {
        animator.Play(NAIL_AOE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailAOECollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swingDuration);
        nailAOECollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
