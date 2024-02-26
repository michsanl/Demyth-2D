using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Core;
using Demyth.Gameplay;

public class SriAbilityDownSlash : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _downSlashProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject downSlashCollider;
    
    private Player _player;
    private int topArenaBorder = 2;
    private int bottomArenaBorder = -4;
    private int DOWN_SLASH = Animator.StringToHash("Down_Slash");

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Ver_Slash_Multiplier", _downSlashProp.AnimationSpeedMultiplier);
        
        var playerYPosition = _player.transform.position.y;
        var targetPosition = ClampValueToBattleArenaBorder(GetPositionWithIncrement(playerYPosition));
        int finalTargetPosition = Mathf.RoundToInt(targetPosition);

        _animator.SetTrigger(DOWN_SLASH);
        Helper.PlaySFX(_sriClipSO.VerticalSlash, _sriClipSO.VerticalSlashVolume);

        yield return Helper.GetWaitForSeconds(_downSlashProp.GetFrontSwingDuration());
        downSlashCollider.SetActive(true);
        yield return transform.DOMoveY(finalTargetPosition, _downSlashProp.GetSwingDuration()).SetEase(animationCurve).WaitForCompletion();
        downSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(_downSlashProp.GetBackSwingDuration());
    }

    private float GetPositionWithIncrement(float playerYPosition)
    {
        return playerYPosition - 2;
    }

    private float ClampValueToBattleArenaBorder(float value)
    {
        return Mathf.Clamp(value, bottomArenaBorder, topArenaBorder);
    }
}
