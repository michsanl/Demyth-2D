using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using System;

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
    
    
    public IEnumerator BasicSlam(Action OnStart)
    {
        OnStart?.Invoke();

        yield return Helper.GetWaitForSeconds(frontSwingDuration);

        var coffinSpawnPosition = Context.Player.LastMoveTargetPosition;
        Instantiate(groundCoffin, coffinSpawnPosition, Quaternion.identity);
        
        basicSlamCollider.SetActive(true);

        yield return Helper.GetWaitForSeconds(swingDuration);

        basicSlamCollider.SetActive(false);

        yield return Helper.GetWaitForSeconds(backSwingDuration);
    }
}
