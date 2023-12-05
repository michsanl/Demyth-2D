using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using Core;
using MoreMountains.Tools;

public class PetraAbilityJumpSlam : MonoBehaviour
{
    [Title("Animation Timeline")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Jump Parameter")]
    [SerializeField] private float jumpPower;
    [SerializeField] private AnimationCurve jumpCurve;
    
    [Title("Components")]
    [SerializeField] private GameObject jumpSlamCollider;

    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");

    public IEnumerator JumpSlam(Vector2 targetPosition, Animator animator, AudioClip abilitySFX)
    {
        animator.Play(JUMP_SLAM);
        PlayAudio(abilitySFX);
        
        yield return Helper.GetWaitForSeconds(frontSwingDuration);

        yield return transform.DOJump(targetPosition, jumpPower, 1, swingDuration).SetEase(jumpCurve).WaitForCompletion();

        jumpSlamCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(backSwingDuration);

        jumpSlamCollider.SetActive(false);
    }

    private void PlayAudio(AudioClip abilitySFX)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = 1f;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        MMSoundManagerSoundPlayEvent.Trigger(abilitySFX, playOptions);
    }
}
