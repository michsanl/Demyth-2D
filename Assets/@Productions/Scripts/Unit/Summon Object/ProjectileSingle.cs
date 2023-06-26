using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSingle : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3 shootDir;
    
    public void SetupDirection(Vector3 direction)
    {
        shootDir = Vector3.right;
        transform.eulerAngles = new Vector3(0, 0, Helper.GetAngleFromFectorFloat(shootDir));
        Destroy(gameObject, 3f);
    }

    private void Update() 
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;    
    }
}
