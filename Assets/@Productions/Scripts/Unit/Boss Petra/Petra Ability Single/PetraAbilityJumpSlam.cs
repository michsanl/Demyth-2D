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
    [SerializeField] private float colliderDelayTime;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject jumpSlamCollider;
    
    private int JUMP_SLAM = Animator.StringToHash("Jump_slam");
    
    public IEnumerator JumpSlam(Vector2 targetPosition)
    {
        animator.Play(JUMP_SLAM);
        // audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);

        transform.position = targetPosition;
        StartCoroutine(SetActiveColliderWithDelay(jumpSlamCollider, colliderDelayTime));
        yield return Helper.GetWaitForSeconds(swingDuration);

        jumpSlamCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private IEnumerator SetActiveColliderWithDelay(GameObject colliderGameObject, float delayTime)
    {
        yield return Helper.GetWaitForSeconds(delayTime);
        colliderGameObject.SetActive(true);
    }
}
