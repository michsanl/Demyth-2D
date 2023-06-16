using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedNail : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject anticipationModel;
    [SerializeField] private GameObject colliderGO;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float summonDelayTimer;

    private void Awake() 
    {
        StartCoroutine(SpawnNailModel());
        StartCoroutine(SpawnCollider());
        StartCoroutine(DestroySelf());
    }

    private IEnumerator SpawnNailModel()
    {
        yield return Helper.GetWaitForSeconds(summonDelayTimer);
        model.SetActive(true);
        anticipationModel.SetActive(false);
    }

    private IEnumerator SpawnCollider()
    {
        yield return Helper.GetWaitForSeconds(summonDelayTimer + 0.1333f);
        colliderGO.SetActive(true);
    }

    private IEnumerator DestroySelf()
    {   
        yield return Helper.GetWaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }
}
