using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PetraAbilityBasicSlam : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject basicSlamCollider;
    [SerializeField] private GameObject groundCoffin;
    
    private int BASIC_SLAM = Animator.StringToHash("Basic_Slam");
    
    public IEnumerator BasicSlam(Vector2 targetPosition)
    {
        animator.Play(BASIC_SLAM);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        basicSlamCollider.SetActive(true);
        Instantiate(groundCoffin, targetPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(swingDuration);
        basicSlamCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
