using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SriAbilityNailSummon : MonoBehaviour
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    
    protected int NAIL_SUMMON_SINGLE = Animator.StringToHash("Nail_Summon_Single");

    public IEnumerator NailSummon(Player player)
    {
        var targetPosition = player.LastMoveTargetPosition;

        animator.Play(NAIL_SUMMON_SINGLE);
        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailSummonCollider.SetActive(true);
        Instantiate(groundNail, targetPosition, Quaternion.identity);
        yield return Helper.GetWaitForSeconds(swingDuration);
        nailSummonCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
