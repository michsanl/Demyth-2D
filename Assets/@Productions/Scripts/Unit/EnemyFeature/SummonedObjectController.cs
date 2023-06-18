using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SummonedObjectController : MonoBehaviour
{
    [SerializeField] private float destroySelfTimer;
    [Space]
    [SerializeField] private float colliderSpawnDelay;
    [SerializeField] private GameObject colliderGameObject;

    private void Awake() 
    {
        StartCoroutine(SpawnColliderWithDelay());
        Destroy(gameObject, destroySelfTimer);
    }

    private IEnumerator SpawnColliderWithDelay()
    {
        yield return Helper.GetWaitForSeconds(colliderSpawnDelay);
        colliderGameObject.SetActive(true);
    }
}
