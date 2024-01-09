using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarShine : MonoBehaviour
{
    [SerializeField] private PillarLight pillarLight;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        pillarLight.OnTurnOnLight += OnTurnOnLight;
    }

    private void OnTurnOnLight()
    {
        animator.SetBool("Shine", false);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other == null) return;
        if (pillarLight.IsLightActive) return;

        animator.SetBool("Shine", true);
    }

    private void OnCollisionStay(Collision other) 
    {
        if (other == null) return;
        if (pillarLight.IsLightActive) return;

        animator.SetBool("Shine", true);
    }

    private void OnCollisionExit(Collision other) 
    {
        if (other == null) return;
        if (pillarLight.IsLightActive) return;

        animator.SetBool("Shine", false);
    }
}
