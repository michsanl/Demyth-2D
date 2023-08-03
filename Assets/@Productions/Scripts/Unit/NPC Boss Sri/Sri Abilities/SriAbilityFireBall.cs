using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;

public class SriAbilityFireBall : SceneService
{
    [Title("Parameter Settings")]
    // [SerializeField] private float frontSwingDuration;
    // [SerializeField] private float swingDuration;
    // [SerializeField] private float backSwingDuration;
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fireBallProjectile;
    [SerializeField] private Transform fireBallSpawnPosition;
    
    protected int FIRE_BALL = Animator.StringToHash("Fire_Ball");

    public IEnumerator FireBall()
    {
        var audioManager = Context.AudioManager;

        animator.Play(FIRE_BALL);
        // audioManager.PlaySound(audioManager.SriAudioSource.Fireball);

        Instantiate(fireBallProjectile, fireBallSpawnPosition.position, Quaternion.identity);
        yield return Helper.GetWaitForSeconds(animationDuration);
    }
}
