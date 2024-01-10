using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiddenItemShine : MonoBehaviour
{
    [SerializeField] private GameObject pickupableGameOBject;
    [SerializeField] private Animator animator;

    private void OnCollisionEnter(Collision other) 
    {
        animator.SetBool("Shine", true);
        pickupableGameOBject.SetActive(true);
    }

    private void OnCollisionExit(Collision other) 
    {
        animator.SetBool("Shine", false);
        pickupableGameOBject.SetActive(false);
    }

}
