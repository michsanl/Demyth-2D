using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class PetraAbilityBasicSlam : SceneService
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
    
    public IEnumerator BasicSlam()
    {
        var audioManager = Context.AudioManager;
        var coffinSpawnPosition = Context.Player.LastMoveTargetPosition;
        animator.Play(BASIC_SLAM);
        audioManager.PlaySound(audioManager.PetraAudioSO.BasicSlam);

        yield return Helper.GetWaitForSeconds(frontSwingDuration);
        Instantiate(groundCoffin, coffinSpawnPosition, Quaternion.identity);
        basicSlamCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);

        basicSlamCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
