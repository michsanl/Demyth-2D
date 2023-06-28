using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityNailAOE : SceneService
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
        var audioManager = Context.AudioManager;

        if (summonProjectile)
            Instantiate(nailProjectile, transform.position, Quaternion.identity);

        animator.Play(NAIL_AOE);
        audioManager.PlayClipAtPoint(audioManager.SriAudioSource.NailAOE, transform.position);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailAOECollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swingDuration);
        nailAOECollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
