using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSummonMultiWave : MonoBehaviour
{
    [SerializeField] private float destroySelfDelay;
    [SerializeField] private float secondWaveDelay;
    [SerializeField] private GameObject secondWave;

    private void Awake() 
    {
        StartCoroutine(Spawn());
        Destroy(gameObject, destroySelfDelay);
    }

    private IEnumerator Spawn()
    {
        yield return Helper.GetWaitForSeconds(secondWaveDelay);
        secondWave.SetActive(true);
    }
}
