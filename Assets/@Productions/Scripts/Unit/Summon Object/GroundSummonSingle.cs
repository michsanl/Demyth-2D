using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSummonSingle : MonoBehaviour
{
    [SerializeField] private float maxRandomSpawnDelay = 0.1f;
    [SerializeField] private float colliderSpawnDelay;
    [SerializeField] private float destroySelfDelay;
    [Space]
    [SerializeField] private GameObject colliderGameObject;
    [SerializeField] private GameObject[] groundCoffinModelArray;

    private void Awake()
    {
        StartCoroutine(SummonRoutine());
    }

    private IEnumerator SummonRoutine()
    {
        var randomRoutineDelay = Random.Range(0, maxRandomSpawnDelay);
        yield return Helper.GetWaitForSeconds(randomRoutineDelay);

        SpawnRandomModel();
        StartCoroutine(SpawnColliderWithDelay());
        Destroy(gameObject, destroySelfDelay);
    }

    private void SpawnRandomModel()
    {
        var randomIndex = Random.Range(0,3);
        groundCoffinModelArray[randomIndex].SetActive(true);
    }

    private IEnumerator SpawnColliderWithDelay()
    {
        yield return Helper.GetWaitForSeconds(colliderSpawnDelay);
        colliderGameObject.SetActive(true);
    }
}
