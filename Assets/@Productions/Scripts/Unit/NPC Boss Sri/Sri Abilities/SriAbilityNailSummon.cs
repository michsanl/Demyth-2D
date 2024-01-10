using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityNailSummon : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _nailSummonProp;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    [SerializeField] private SriClipSO _sriClipSO;
    
    protected int NAIL_SUMMON = Animator.StringToHash("Nail_Summon");

    public IEnumerator NailSummon(Player player, Animator animator)
    {
        animator.SetFloat("Nail_Summon_Multiplier", _nailSummonProp.AnimationSpeedMultiplier);
        
        animator.SetTrigger(NAIL_SUMMON);
        Helper.PlaySFX(_sriClipSO.NailSummon, _sriClipSO.NailSummonVolume);
        
        Vector2 spawnPosition = player.LastMoveTargetPosition;
        LeanPool.Spawn(groundNail, spawnPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetFrontSwingDuration());
        nailSummonCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetSwingDuration());
        nailSummonCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_nailSummonProp.GetBackSwingDuration());
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }

    private IEnumerator HandleSpawnGroundNail(Player player)
    {
        yield return Helper.GetWaitForSeconds(nailSpawnDelay);
        
        Vector2 spawnPosition = player.LastMoveTargetPosition;
        Instantiate(groundNail, spawnPosition, Quaternion.identity);
    }
}
