using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float timer;

    private void Start() 
    {
        StartCoroutine(DestroyMe());
    }

    public IEnumerator DestroyMe()
    {
        yield return Helper.GetWaitForSeconds(timer);
        Destroy(gameObject);
    }
}
