using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedNail : MonoBehaviour
{
    [SerializeField] private GameObject nailModel;
    [SerializeField] private GameObject anticipationModel;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float summonDelayTimer;

    private void Start() 
    {
        StartCoroutine(DestroyMe());
    }

    public IEnumerator DestroyMe()
    {
        yield return Helper.GetWaitForSeconds(summonDelayTimer);
        anticipationModel.SetActive(false);
        nailModel.SetActive(true);
        
        yield return Helper.GetWaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }
}
