using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedNail : MonoBehaviour
{
    [SerializeField] private GameObject colliderGameObject;
    [SerializeField] private float timer;
    [SerializeField] private float summonDelay;

    private void Start() 
    {
        StartCoroutine(DestroyMe());
    }

    public IEnumerator DestroyMe()
    {
        yield return Helper.GetWaitForSeconds(summonDelay);
        colliderGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(timer-summonDelay);
        Destroy(gameObject);
    }
}
