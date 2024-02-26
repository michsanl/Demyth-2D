using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityWaveOutNailWave : Ability
{
    [Title("Parameter Settings")]
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _introProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject waveOutNailWave;
    
    private float teleportStartDuration = 0.3f;
    private float teleportEndDuration = 0.4f;
    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private int NAIL_WAVE = Animator.StringToHash("Intro");

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Nail_AOE_Multiplier", _introProp.AnimationSpeedMultiplier);

        yield return StartCoroutine(TeleportToMiddleArena());

        _animator.SetTrigger(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        StartCoroutine(SpawnNail());

        yield return Helper.GetWaitForSeconds(_introProp.GetSwingDuration());
    }

    public IEnumerator TeleportToMiddleArena()
    {
        _animator.Play(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(teleportStartDuration);

        transform.position = new Vector3(0, -1, 0);

        _animator.Play(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(teleportEndDuration);
    }

    private IEnumerator SpawnNail()
    {
        yield return Helper.GetWaitForSeconds(nailSpawnDelay);

        LeanPool.Spawn(waveOutNailWave, new Vector3(0, -1, 0), Quaternion.identity);
    }
}
