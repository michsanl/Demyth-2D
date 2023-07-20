using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProximityShine : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public Action OnShineOn;
    public Action OnShineOff;

    private void OnCollisionEnter(Collision other) 
    {
        OnShineOn?.Invoke();
        animator.SetBool("Shine", true);
    }

    private void OnCollisionExit(Collision other) 
    {
        animator.SetBool("Shine", false);
        OnShineOn?.Invoke();
    }

}
