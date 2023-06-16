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
    [SerializeField] private float newColorDuration;
    [OnValueChanged("CalculateFlashDuration")]
    [SerializeField] private float normalColorDuration;
    [OnValueChanged("CalculateFlashDuration")]
    [SerializeField] private int cycleAmount;
    
    [Space]
    [ReadOnly]
    public float FlashEffectDuration;

    private MaterialPropertyBlock mpbNormal;
    private MaterialPropertyBlock mpbNew;

    private void Awake() 
    {
        mpbNormal = new MaterialPropertyBlock();
        mpbNew = new MaterialPropertyBlock();

        Color newColor = new Color(255, 255, 255, 0);

        mpbNew.SetColor("_RendererColor", newColor);
        mpbNew.SetColor("_Color", newColor);
    }

    public IEnumerator PlayFlashEffect()
    {
        for (int i = 0; i < cycleAmount; i++)
        {
            spineMeshRenderer.SetPropertyBlock(mpbNew);
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
