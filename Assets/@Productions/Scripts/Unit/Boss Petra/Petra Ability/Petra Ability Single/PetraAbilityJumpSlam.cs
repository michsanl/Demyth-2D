using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityJumpSlam : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [SerializeField] private float jumpPower;
    [SerializeField] private float colliderDelayTime;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject jumpSlamCollider;
    
    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");
    
    public IEnumerator JumpSlam(Vector2 targetPosition)
    {
        animator.Play(JUMP_SLAM);
        // audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        
        StartCoroutine(SetActiveColliderWithDelay(jumpSlamCollider, colliderDelayTime));
        yield return transform.DOJump(targetPosition, jumpPower, 1, swingDuration).SetEase(animationCurve).WaitForCompletion();

        yield return Helper.GetWaitForSeconds(backSwingDuration);
        jumpSlamCollider.SetActive(false);
    }

    private IEnumerator SetActiveColliderWithDelay(GameObject colliderGameObject, float delayTime)
    {
        yield return Helper.GetWaitForSeconds(delayTime);
        colliderGameObject.SetActive(true);
    }
}
