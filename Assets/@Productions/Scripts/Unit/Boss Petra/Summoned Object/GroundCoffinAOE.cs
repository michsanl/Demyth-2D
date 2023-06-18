using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCoffinAOE : MonoBehaviour
{
    [SerializeField] private float destroySelfDelay = 1.5f;
    [SerializeField] private float waveSpawnDelay;
    [SerializeField] private GameObject targetWaveGameObject;

    private void Awake() 
    {
        StartCoroutine(Spawn());
        Destroy(gameObject, destroySelfDelay);
    }

    private IEnumerator Spawn()
    {
        yield return Helper.GetWaitForSeconds(waveSpawnDelay);
        targetWaveGameObject.SetActive(true);
    }
}
