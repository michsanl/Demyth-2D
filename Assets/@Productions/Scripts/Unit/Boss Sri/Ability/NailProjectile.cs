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
        transform.eulerAngles = new Vector3(0, 0, Helper.GetAngleFromFectorFloat(shootDir));
        Destroy(gameObject, 3f);
    }

    private void Update() 
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;    
    }
}
