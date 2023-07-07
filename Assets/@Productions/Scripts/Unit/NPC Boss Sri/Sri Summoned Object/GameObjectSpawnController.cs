using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawnController : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float destroyTimerOnComplete = 2f;
    [SerializeField] private GameObject[] gameObjectsToSpawnArray;

    private void Awake()
    {
        StartCoroutine(RelaySpawn());
    }

    private IEnumerator RelaySpawn()
    {
        foreach (var gameObject in gameObjectsToSpawnArray)
        {
            gameObject.gameObject.SetActive(true);
            yield return Helper.GetWaitForSeconds(spawnInterval);
        }

        Destroy(gameObject, destroyTimerOnComplete);
    }
}
