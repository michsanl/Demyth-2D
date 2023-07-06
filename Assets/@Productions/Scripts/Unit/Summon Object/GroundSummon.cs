using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GroundSummon : MonoBehaviour
{
    [SerializeField] private float colliderSpawnDelay;
    [SerializeField] private float destroySelfDelay;

    [Title("Randomized Spawn")]
    [SerializeField] private bool useRandomizedSpawnDelay;
    [ShowIf("useRandomizedSpawnDelay")]
    [SerializeField] private float minRandomSpawnDelay;
    [ShowIf("useRandomizedSpawnDelay")]
    [SerializeField] private float maxRandomSpawnDelay = 0.1f;

    [Title("Combat Arena Size")]
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
        if (useRandomizedSpawnDelay)
            yield return StartCoroutine(RandomizedSpawnDelay());

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

    private IEnumerator RandomizedSpawnDelay()
    {
        var delayTime = Random.Range(minRandomSpawnDelay, maxRandomSpawnDelay);
        yield return Helper.GetWaitForSeconds(delayTime);
    }

    private void DisplayRandomModel()
    {
        var randomIndex = Random.Range(0, groundCoffinModelArray.Length);
        groundCoffinModelArray[randomIndex].SetActive(true);
    }

    private IEnumerator SpawnColliderWithDelay()
    {
        yield return Helper.GetWaitForSeconds(colliderSpawnDelay);
        colliderGameObject.SetActive(true);
    }
}
