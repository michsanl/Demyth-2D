using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SriAbilitySpinClaw : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private float _sfxDelay;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _spinClawProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject spinClawCollider;
    
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Spin_Claw_Multiplier", _spinClawProp.AnimationSpeedMultiplier);

        _animator.SetTrigger(SPIN_CLAW);
        StartCoroutine(PlaySFX());

        yield return Helper.GetWaitForSeconds(_spinClawProp.GetFrontSwingDuration());
        spinClawCollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(_spinClawProp.GetSwingDuration());
        spinClawCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_spinClawProp.GetBackSwingDuration());
    }

    private IEnumerator PlaySFX()
    {
        yield return Helper.GetWaitForSeconds(_sfxDelay);
        Helper.PlaySFX(_sriClipSO.SpinClaw, _sriClipSO.SpinClawVolume);
    }
}
