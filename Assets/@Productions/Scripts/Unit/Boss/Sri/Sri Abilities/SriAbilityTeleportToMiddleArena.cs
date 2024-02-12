using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class SriAbilityTeleportToMiddleArena : Ability
{
    [SerializeField] private AnimationPropertiesSO _teleportProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    
    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private Vector2 _teleportTargetPosition = new(0, 1);

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Teleport_Multiplier", _teleportProp.AnimationSpeedMultiplier);

        _animator.Play(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetFrontSwingDuration());

        transform.position = _teleportTargetPosition;

        _animator.Play(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetBackSwingDuration());
    }
}
