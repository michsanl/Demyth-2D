using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    
    [SerializeField] private float destroyTimer;
    [SerializeField] private GameObject[] hitEffectModelArray;

    private void Awake() 
    {
        ActivateRandomModel();
        Destroy(gameObject, destroyTimer);
    }

    private void ActivateRandomModel()
    {
        var randomIndex = UnityEngine.Random.Range(0, hitEffectModelArray.Length);
        hitEffectModelArray[randomIndex].SetActive(true);
    }
}
