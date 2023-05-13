using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// set game object layer to Pickupable
// Pickupable layer only collide with Player layer
public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;

    private void OnTriggerEnter2D(Collider2D col) 
    {
        Debug.Log(col.name);
        Destroy(mainGameObject.gameObject);
    }
}
