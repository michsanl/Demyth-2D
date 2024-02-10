using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Lean.Pool;
using Core;
using Demyth.Gameplay;

public class SriAbilityNailSummon : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _nailSummonProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    
    private Player _player;
    protected int NAIL_SUMMON = Animator.StringToHash("Nail_Summon");

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Nail_Summon_Multiplier", _nailSummonProp.AnimationSpeedMultiplier);
        
        _animator.SetTrigger(NAIL_SUMMON);
        Helper.PlaySFX(_sriClipSO.NailSummon, _sriClipSO.NailSummonVolume);
        
        Vector2 spawnPosition = _player.LastMoveTargetPosition;
        LeanPool.Spawn(groundNail, spawnPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetFrontSwingDuration());
        nailSummonCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetSwingDuration());
        nailSummonCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetBackSwingDuration());
    }
}
