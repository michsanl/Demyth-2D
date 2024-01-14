using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using CustomTools.Core;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityFireBall : MonoBehaviour
{
    [Title("Parameter Settings")]
    // [SerializeField] private float frontSwingDuration;
    // [SerializeField] private float swingDuration;
    // [SerializeField] private float backSwingDuration;
    [SerializeField] private float animationDuration;
    
    [Title("Components")]
    [SerializeField] private AnimationPropertiesSO _fireBallProp;
    [SerializeField] private GameObject fireBallProjectile;
    [SerializeField] private Transform fireBallSpawnPosition;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private float sfxDuration;
    [SerializeField] private float fadeDuration;

    protected int FIRE_BALL = Animator.StringToHash("Fire_Ball");

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator FireBall(Animator animator)
    {
        animator.SetFloat("Fire_Ball_Multiplier", _fireBallProp.AnimationSpeedMultiplier);
        
        animator.SetTrigger(FIRE_BALL);
        StartCoroutine(PlayFireballSFX());

        LeanPool.Spawn(fireBallProjectile, fireBallSpawnPosition.position, Quaternion.identity);
        
        yield return Helper.GetWaitForSeconds(_fireBallProp.GetSwingDuration());
    }

    private IEnumerator PlayFireballSFX()
    {
        var source = Helper.PlaySFX(_sriClipSO.Fireball, _sriClipSO.FireballVolume);

        yield return Helper.GetWaitForSeconds(sfxDuration);

        StartCoroutine(StartFadeCoroutine(source, fadeDuration, 0));
    }

    public IEnumerator StartFadeCoroutine(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
