using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarShine : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnCollisionEnter(Collision other) 
    {
        animator.SetBool("Shine", true);
    }

    private void OnCollisionExit(Collision other) 
    {
        animator.SetBool("Shine", false);
    }
}
