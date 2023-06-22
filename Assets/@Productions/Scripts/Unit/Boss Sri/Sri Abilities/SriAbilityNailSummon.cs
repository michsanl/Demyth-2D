using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityNailSummon : SceneService
{
    [Title("Parameter Settings")]
    [SerializeField] private float frontSwingDuration;
    [SerializeField] private float swingDuration;
    [SerializeField] private float backSwingDuration;
    [InfoBox("Value cannot exceed FrontSwingDuration", InfoMessageType.Warning)]
    [SerializeField] private float nailPositionAcquireDelay;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    
    protected int NAIL_SUMMON_SINGLE = Animator.StringToHash("Nail_Summon_Single");

    public IEnumerator NailSummon(Player player)
    {
        animator.Play(NAIL_SUMMON_SINGLE);
        StartCoroutine(HandleSpawnGroundNail());

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailSummonCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);
        nailSummonCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private IEnumerator HandleSpawnGroundNail()
    {
        yield return Helper.GetWaitForSeconds(nailPositionAcquireDelay);
        Vector2 spawnPosition = Context.Player.LastMoveTargetPosition;
        yield return Helper.GetWaitForSeconds(frontSwingDuration - nailPositionAcquireDelay);
        Instantiate(groundNail, spawnPosition, Quaternion.identity);
    }
}
