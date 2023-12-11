using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;

public class SriAbilityNailSummon : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    
    protected int NAIL_SUMMON_SINGLE = Animator.StringToHash("Nail_Summon_Single");

    public IEnumerator NailSummon(Player player, Animator animator, AudioClip abilitySFX)
    {
        animator.CrossFade(NAIL_SUMMON_SINGLE, .1f, 0);
        PlayAudio(abilitySFX);
        
        Vector2 spawnPosition = player.LastMoveTargetPosition;
        Instantiate(groundNail, spawnPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailSummonCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);
        nailSummonCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
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
