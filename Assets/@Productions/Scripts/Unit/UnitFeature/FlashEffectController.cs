using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class FlashEffectController : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer spineMeshRenderer;
    [SerializeField] private MMF_Player _flashEffectMMFPlayer;
    [SerializeField] private float _feedbackDuration;
    
    [ReadOnly]
    public float HitEffectBlend;  // public so MMFeedback can access
    private Material _material;
    private bool _canUpdateFlashEffect;

    private void Update()
    {
        UpdateFlashEffectBlend();
    }

    private void OnEnable() 
    {
        ResetCondition();
    }

    private void OnDisable()
    {
        ResetCondition();
    }

    public void PlayFlashEffect()
    {
        StartCoroutine(PlayFlashEffectCoroutine());
    }

    private void UpdateFlashEffectBlend()
    {
        if (!_canUpdateFlashEffect)
            return;

        _material = spineMeshRenderer.material;
        _material.SetFloat("_HitEffectBlend", HitEffectBlend);
    }

    private IEnumerator PlayFlashEffectCoroutine()
    {
        _canUpdateFlashEffect = true;

        // Tween blend value with MMFeedback
        _flashEffectMMFPlayer.PlayFeedbacks();
        yield return Helper.GetWaitForSeconds(_feedbackDuration);
        ResetCondition();

        _canUpdateFlashEffect = false;
    }

    private void ResetCondition()
    {
        _canUpdateFlashEffect = false;
        HitEffectBlend = 0;
        _material = spineMeshRenderer.material;
        _material.SetFloat("_HitEffectBlend", HitEffectBlend);
    }





    ////////////// OLD METHOD ///////////////////

    // [Title("Parameter Settings")]

    // [OnValueChanged("CalculateFlashDuration")]
    // [SerializeField] private float newColorDuration = 0.1f;
    // [OnValueChanged("CalculateFlashDuration")]
    // [SerializeField] private float normalColorDuration = 0.2f;
    // [OnValueChanged("CalculateFlashDuration")]
    // [SerializeField] private int cycleAmount = 4;
    // [Space]
    // [ReadOnly] public float HitEffectBlend;
    // [ReadOnly] public float FlashEffectDuration;

    // private Player player;
    // private MaterialPropertyBlock mpbNormal;
    // private MaterialPropertyBlock mbpFlash;
    // private Coroutine activeFlashEffectCoroutine;

    // private void Awake()
    // {
    //     player = GetComponent<Player>();
    //     SetupFlashMBP();
    // }

    // private void Start() 
    // {
    //     player.OnInvulnerableVisualStart += Player_OnInvulnerableStart;
    //     player.OnInvulnerableVisualEnd += Player_OnInvulnerableEnd;
    // }

    // private void SetupFlashMBP()
    // {
    //     mpbNormal = new MaterialPropertyBlock();
    //     mbpFlash = new MaterialPropertyBlock();

    //     Color flashColor = new Color(255, 255, 255, 0);

    //     mbpFlash.SetColor("_RendererColor", flashColor);
    //     mbpFlash.SetColor("_Color", flashColor);
    // }

    // private void Player_OnInvulnerableStart()
    // {
    //     activeFlashEffectCoroutine = StartCoroutine(PlayEndlessFlashEffect());
    // }

    // private void Player_OnInvulnerableEnd()
    // {
    //     StopCoroutine(activeFlashEffectCoroutine);
    //     spineMeshRenderer.SetPropertyBlock(mpbNormal);
    // }

    // public IEnumerator PlayFlashEffect()
    // {
    //     for (int i = 0; i < cycleAmount; i++)
    //     {
    //         spineMeshRenderer.SetPropertyBlock(mbpFlash);
    //         yield return Helper.GetWaitForSeconds(newColorDuration);
    //         spineMeshRenderer.SetPropertyBlock(mpbNormal);
    //         yield return Helper.GetWaitForSeconds(normalColorDuration);
    //     }
    // }

    // private IEnumerator PlayEndlessFlashEffect()
    // {
    //     for (int i = 0; i < cycleAmount; i++)
    //     {
    //         spineMeshRenderer.SetPropertyBlock(mbpFlash);
    //         yield return Helper.GetWaitForSeconds(newColorDuration);
    //         spineMeshRenderer.SetPropertyBlock(mpbNormal);
    //         yield return Helper.GetWaitForSeconds(normalColorDuration);
    //     }
    // }

    // private void CalculateFlashDuration()
    // {
    //     FlashEffectDuration = (newColorDuration + normalColorDuration) * cycleAmount;
    // }

}
