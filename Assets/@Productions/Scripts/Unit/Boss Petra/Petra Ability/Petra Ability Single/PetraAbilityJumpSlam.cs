using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityJumpSlam : MonoBehaviour
{
    [Title("Animation Timeline")]
    [SerializeField] private float animationDuration;
    [Space]
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

    public IEnumerator JumpSlam(Vector2 targetPosition)
    {
        StartCoroutine(SetCollider());
        StartCoroutine(JumpToLocation(targetPosition));

        animator.Play(JUMP_SLAM);
        yield return Helper.GetWaitForSeconds(animationDuration);

        jumpSlamCollider.SetActive(false);
    }

    private IEnumerator JumpToLocation(Vector3 targetPosition)
    {
        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        yield return transform.DOJump(targetPosition, jumpPower, 1, swingDuration).SetEase(jumpCurve).WaitForCompletion();
    }

    private IEnumerator SetCollider()
    {
        yield return Helper.GetWaitForSeconds(frontSwingDuration + swingDuration);
        jumpSlamCollider.SetActive(true);
    }
}
