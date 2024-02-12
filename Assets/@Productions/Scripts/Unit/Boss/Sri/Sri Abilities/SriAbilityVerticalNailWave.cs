using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityVerticalNailWave : Ability
{
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _introProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject verticalNailWave;
    
    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Nail_AOE_Multiplier", _introProp.AnimationSpeedMultiplier);
        
        _animator.SetTrigger(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        LeanPool.Spawn(verticalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_introProp.GetSwingDuration());
    }
}
