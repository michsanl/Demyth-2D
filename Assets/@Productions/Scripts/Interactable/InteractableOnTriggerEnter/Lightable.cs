using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// set game object layer to Lightable
// Lightable layer only collide with Senter layer
public class Lightable : MonoBehaviour
{
    [SerializeField] private GameObject lightEffectGameObject;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D col) 
    {
        lightEffectGameObject.SetActive(true);
        animator.SetBool("isOn", true);
    }

    private void OnTriggerExit2D(Collider2D col) 
    {
        animator.SetBool("isOn", false);
        lightEffectGameObject.SetActive(false);
    }

}
