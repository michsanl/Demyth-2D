using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class PetraAbilityJumpGroundSlam : MonoBehaviour
{
    [Title("Animation Timeline")]
    [SerializeField] private float _frontSwingDuration = 0.41f;
    [SerializeField] private float _swingDuration = 0.3f;
    [SerializeField] private float _backSwingDuration = 1f;
    
    [Title("Jump Parameter")]
    [SerializeField] private float jumpPower;
    [SerializeField] private AnimationCurve jumpCurve;
    
    [Title("Components")]
    [SerializeField] private GameObject jumpSlamCollider;
    [SerializeField] private GameObject groundCoffinWaveOut;    

    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");

    public IEnumerator JumpGroundSlam(Animator animator, PetraClipSO petraClipSO)
    {
        Vector3 jumpTargetPosition = new Vector3(0, -1, 0);

        animator.Play(JUMP_SLAM);
        Helper.PlaySFX(petraClipSO.JumpSlam, petraClipSO.JumpSlamVolume);
        
        yield return Helper.GetWaitForSeconds(_frontSwingDuration);

        yield return transform.DOJump(jumpTargetPosition, jumpPower, 1, _swingDuration).SetEase(jumpCurve).WaitForCompletion();

        jumpSlamCollider.SetActive(true);
        LeanPool.Spawn(groundCoffinWaveOut, transform.position, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(_backSwingDuration);

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
