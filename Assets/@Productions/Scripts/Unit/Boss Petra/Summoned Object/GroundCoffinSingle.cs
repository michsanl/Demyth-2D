using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GroundCoffinSingle : MonoBehaviour
{
    [SerializeField] private float maxSpawnDelay = 0.1f;
    [SerializeField] private float destroySelfDelay;
    [SerializeField] private float colliderSpawnDelay;
    [Space]
    [SerializeField] private GameObject[] groundCoffinModelArray = new GameObject[3];
    [SerializeField] private GameObject colliderGameObject;

    private void Awake()
    {
        StartCoroutine(SummonRoutine());
    }

    private IEnumerator SummonRoutine()
    {
        var randomRoutineDelay = Random.Range(0, maxSpawnDelay);

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
