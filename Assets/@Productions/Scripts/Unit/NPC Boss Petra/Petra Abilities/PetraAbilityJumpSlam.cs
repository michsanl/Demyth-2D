using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityJumpSlam : SceneService
{
    [Title("Animation Timeline")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Jump Parameter")]
    [SerializeField] private float jumpPower;
    [SerializeField] private AnimationCurve jumpCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject jumpSlamCollider;

    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");

    public IEnumerator JumpSlam()
    {
        animator.Play(JUMP_SLAM);
        var audioManager = Context.AudioManager;
        audioManager.PlaySound(audioManager.PetraAudioSource.JumpSlam);
        Vector3 targetPosition = Context.Player.LastMoveTargetPosition;
        
        yield return Helper.GetWaitForSeconds(frontSwingDuration);

        yield return transform.DOJump(targetPosition, jumpPower, 1, swingDuration).SetEase(jumpCurve).WaitForCompletion();

        jumpSlamCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(backSwingDuration);

        jumpSlamCollider.SetActive(false);
    }
}
