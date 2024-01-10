using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnDeath : MonoBehaviour
{
    
    private Health health;

    private void Start() 
    {
        health = GetComponent<Health>();
        health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath()
    {
        gameObject.SetActive(false);
    }
}
