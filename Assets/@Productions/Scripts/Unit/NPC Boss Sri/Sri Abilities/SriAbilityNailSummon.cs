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
    [SerializeField] private float nailSpawnDelay;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nailSummonCollider;
    [SerializeField] private GameObject groundNail;
    
    protected int NAIL_SUMMON_SINGLE = Animator.StringToHash("Nail_Summon_Single");

    public IEnumerator NailSummon()
    {
        var audioManager = Context.AudioManager;

        animator.CrossFade(NAIL_SUMMON_SINGLE, .1f, 0);
        audioManager.PlaySound(audioManager.SriAudioSO.NailSummon);
        
        Vector2 spawnPosition = Context.Player.LastMoveTargetPosition;
        Instantiate(groundNail, spawnPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        nailSummonCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);
        nailSummonCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }

    private IEnumerator HandleSpawnGroundNail()
    {
        yield return Helper.GetWaitForSeconds(nailSpawnDelay);
        
        Vector2 spawnPosition = Context.Player.LastMoveTargetPosition;
        Instantiate(groundNail, spawnPosition, Quaternion.identity);
    }
}
