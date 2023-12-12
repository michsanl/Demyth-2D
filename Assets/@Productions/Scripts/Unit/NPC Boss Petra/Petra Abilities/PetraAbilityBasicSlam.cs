using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Core;
using Lean.Pool;

public class PetraAbilityBasicSlam : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float _frontSwingDuration = 0.5f;
    [SerializeField] private float _swingDuration = 0.417f;
    [SerializeField] private float _backSwingDuration = 0.333f;
    
    [Title("Components")]
    [SerializeField] private GameObject basicSlamCollider;
    [SerializeField] private GameObject groundCoffin;
    
    private int BASIC_SLAM = Animator.StringToHash("Basic_Slam");

    public IEnumerator BasicSlam(Player player, Animator animator, AudioClip abilitySFX)
    {
        animator.Play(BASIC_SLAM);
        PlayAudio(abilitySFX);

        var coffinSpawnPosition = player.LastMoveTargetPosition;
        
        yield return Helper.GetWaitForSeconds(_frontSwingDuration);

        var spawnedCoffin = LeanPool.Spawn(groundCoffin, coffinSpawnPosition, Quaternion.identity);
        basicSlamCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(_swingDuration);

        basicSlamCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(_backSwingDuration);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
