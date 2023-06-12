using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NailProjectile : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed;

    private Vector3 shootDir;

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromFectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    private void Update() 
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;    
    }

    private float GetAngleFromFectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
