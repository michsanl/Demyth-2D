using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class FlashEffectController : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer spineMeshRenderer;

    [Title("Parameter Settings")]

    [OnValueChanged("CalculateFlashDuration")]
    [SerializeField] private float newColorDuration = 0.1f;
    [OnValueChanged("CalculateFlashDuration")]
    [SerializeField] private float normalColorDuration = 0.2f;
    [OnValueChanged("CalculateFlashDuration")]
    [SerializeField] private int cycleAmount = 4;
    [Space]
    [ReadOnly] public float HitEffectBlend;
    [ReadOnly] public float FlashEffectDuration;

    private Player player;
    private MaterialPropertyBlock mpbNormal;
    private MaterialPropertyBlock mbpFlash;
    private Coroutine activeFlashEffectCoroutine;
    private Material _material;

    private void Awake()
    {
        // player = GetComponent<Player>();
        // SetupFlashMBP();
    }

    private void Start() 
    {
        // player.OnInvulnerableVisualStart += Player_OnInvulnerableStart;
        // player.OnInvulnerableVisualEnd += Player_OnInvulnerableEnd;
    }

    private void Update()
    {
        UpdateHitEffectBlend();
    }

    private void UpdateHitEffectBlend()
    {
        _material = spineMeshRenderer.material;
        _material.SetFloat("_HitEffectBlend", HitEffectBlend);
    }







    private void SetupFlashMBP()
    {
        mpbNormal = new MaterialPropertyBlock();
        mbpFlash = new MaterialPropertyBlock();

        Color flashColor = new Color(255, 255, 255, 0);

        mbpFlash.SetColor("_RendererColor", flashColor);
        mbpFlash.SetColor("_Color", flashColor);
    }

    private void Player_OnInvulnerableStart()
    {
        activeFlashEffectCoroutine = StartCoroutine(PlayEndlessFlashEffect());
    }

    private void Player_OnInvulnerableEnd()
    {
        StopCoroutine(activeFlashEffectCoroutine);
        spineMeshRenderer.SetPropertyBlock(mpbNormal);
    }

    public IEnumerator PlayFlashEffect()
    {
        for (int i = 0; i < cycleAmount; i++)
        {
            spineMeshRenderer.SetPropertyBlock(mbpFlash);
            yield return Helper.GetWaitForSeconds(newColorDuration);
            spineMeshRenderer.SetPropertyBlock(mpbNormal);
            yield return Helper.GetWaitForSeconds(normalColorDuration);
        }
    }

    private IEnumerator PlayEndlessFlashEffect()
    {
        for (int i = 0; i < cycleAmount; i++)
        {
            spineMeshRenderer.SetPropertyBlock(mbpFlash);
            yield return Helper.GetWaitForSeconds(newColorDuration);
            spineMeshRenderer.SetPropertyBlock(mpbNormal);
            yield return Helper.GetWaitForSeconds(normalColorDuration);
        }
    }

    private void CalculateFlashDuration()
    {
        FlashEffectDuration = (newColorDuration + normalColorDuration) * cycleAmount;
    }

}
