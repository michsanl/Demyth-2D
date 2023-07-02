using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GroundSummon : MonoBehaviour
{
    [SerializeField] private float maxRandomSpawnDelay = 0.1f;
    [SerializeField] private float colliderSpawnDelay;
    [SerializeField] private float destroySelfDelay;
    [Title("Arena Size"), Space]
    [SerializeField] private int topBorder;
    [SerializeField] private int bottomBorder;
    [SerializeField] private int rightBorder;
    [SerializeField] private int leftBorder;
    [Space]
    [SerializeField] private GameObject colliderGameObject;
    [SerializeField] private GameObject[] groundCoffinModelArray;

    private void Awake()
    {
        if (IsOutOfBounds())
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SummonRoutine());
        }
    }

    private IEnumerator SummonRoutine()
    {
        yield return StartCoroutine(GetRandomizedSpawnDelay());
        DisplayRandomModel();
        StartCoroutine(SpawnColliderWithDelay());
        Destroy(gameObject, destroySelfDelay);
    }
    
    private bool IsOutOfBounds()
    {
        float positionY = transform.position.y;
        float positionX = transform.position.x;

        return positionY > topBorder || positionY < bottomBorder || positionX > rightBorder || positionX < leftBorder;
    }

    private IEnumerator GetRandomizedSpawnDelay()
    {
        var randomRoutineDelay = Random.Range(0, maxRandomSpawnDelay);
        yield return Helper.GetWaitForSeconds(randomRoutineDelay);
    }

    private void DisplayRandomModel()
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
