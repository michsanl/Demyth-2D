using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demyth.Gameplay;
using Core;
using Lean.Pool;

public class GameObjectSpawnController : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float _despawnTimer = 2f;
    [SerializeField] private bool _disableDespawn;
    [SerializeField] private GameObject[] gameObjectsToSpawnArray;

    private void OnEnable()
    {
        StartCoroutine(ActivateGameObjectWithInterval());
    }

    private void OnDisable()
    {
        foreach (var gameObject in gameObjectsToSpawnArray)
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    private IEnumerator ActivateGameObjectWithInterval()
    {
        foreach (var gameObject in gameObjectsToSpawnArray)
        {
            gameObject.gameObject.SetActive(true);
            yield return Helper.GetWaitForSeconds(spawnInterval);
        }

        if (_disableDespawn) yield break;

        yield return Helper.GetWaitForSeconds(_despawnTimer);
        LeanPool.Despawn(gameObject);
    }
}
