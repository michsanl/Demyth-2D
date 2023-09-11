using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using System;

public class PetraAbilityChargeAttack : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject groundCoffinAOE;
    
    
    public IEnumerator ChargeAttack(Action OnStart)
    {
        OnStart?.Invoke();

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        Instantiate(groundCoffinAOE, transform.position, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(swingDuration);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
