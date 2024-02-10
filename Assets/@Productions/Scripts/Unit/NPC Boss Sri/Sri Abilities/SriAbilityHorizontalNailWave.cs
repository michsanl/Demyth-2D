using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Pool;

public class SriAbilityHorizontalNailWave : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _introProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject horizontalNailWave;

    protected int NAIL_WAVE = Animator.StringToHash("Intro");

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Nail_AOE_Multiplier", _introProp.AnimationSpeedMultiplier);
        
        _animator.SetTrigger(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        LeanPool.Spawn(horizontalNailWave, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_introProp.GetSwingDuration());
    }
}
