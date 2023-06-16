using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlashEffectController : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer spineMeshRenderer;
    [SerializeField] private float flashNewColorDuration;
    [SerializeField] private float flashNormalColorDuration;
    [SerializeField] private int flashCycleCount;
        
    private MaterialPropertyBlock mpbNormal;
    private MaterialPropertyBlock mpbNew;

    private void Awake() 
    {
        mpbNormal = new MaterialPropertyBlock();
        mpbNew = new MaterialPropertyBlock();
        mpbNew.SetColor("_RendererColor", Color.grey * .3f);
    }

    public IEnumerator PlayFlashEffect()
    {
        for (int i = 0; i < flashCycleCount; i++)
        {
            spineMeshRenderer.SetPropertyBlock(mpbNew);
            yield return Helper.GetWaitForSeconds(flashNewColorDuration);
            spineMeshRenderer.SetPropertyBlock(mpbNormal);
            yield return Helper.GetWaitForSeconds(flashNormalColorDuration);
        }
    }
}
