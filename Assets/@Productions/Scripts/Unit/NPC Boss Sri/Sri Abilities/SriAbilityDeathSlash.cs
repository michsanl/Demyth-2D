using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;

public class SriAbilityDeathSlash : Ability
{
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _teleportProp;
    [SerializeField] private AnimationPropertiesSO _nailAOEProp;
    [SerializeField] private AnimationPropertiesSO _downSlashProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject dialogueCollider;
    [SerializeField] private GameObject nailWavePrefab;

    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private int DOWN_SLASH = Animator.StringToHash("Down_Slash");
    private int NAIL_WAVE = Animator.StringToHash("Intro");

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Teleport_Multiplier", _teleportProp.AnimationSpeedMultiplier);
        _animator.SetFloat("Nail_AOE_Multiplier", _nailAOEProp.AnimationSpeedMultiplier);
        _animator.SetFloat("Ver_Slash_Multiplier", _downSlashProp.AnimationSpeedMultiplier);

        // Teleport
        _animator.SetTrigger(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetFrontSwingDuration());

        transform.position = new Vector3(0, 3, 0);

        _animator.SetTrigger(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetBackSwingDuration());

        // Nail Wave
        _animator.SetTrigger(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        yield return Helper.GetWaitForSeconds(.5f / _nailAOEProp.AnimationSpeedMultiplier);
        LeanPool.Spawn(nailWavePrefab, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(3.7f / _nailAOEProp.AnimationSpeedMultiplier);
        
        // Vertical Slash
        _animator.SetTrigger(DOWN_SLASH);
        Helper.PlaySFX(_sriClipSO.VerticalSlash, _sriClipSO.VerticalSlashVolume);
        
        yield return Helper.GetWaitForSeconds(_downSlashProp.GetFrontSwingDuration());
        dialogueCollider.SetActive(true);
        yield return transform.DOMoveY(-4f, _downSlashProp.GetSwingDuration()).SetEase(Ease.OutExpo).WaitForCompletion();
        
        yield return Helper.GetWaitForSeconds(_downSlashProp.GetBackSwingDuration());
        dialogueCollider.SetActive(false);
    }

}
