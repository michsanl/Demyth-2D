using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailProjectileWave : MonoBehaviour
{
    [SerializeField] private float waveSpawnInterval;
    [SerializeField] private GameObject[] waveParentArray;

    private void Awake() 
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        foreach (var waveParent in waveParentArray)
        {
            waveParent.gameObject.SetActive(true);
            yield return Helper.GetWaitForSeconds(waveSpawnInterval);
        }

        Destroy(gameObject, 2f);
    }
}
