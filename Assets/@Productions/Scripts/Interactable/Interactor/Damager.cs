using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Damager : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] bool applyDOTOnStay;
    [SerializeField] float DOTInterval;

    private Interactable interactable;
    private float timer;
    private bool isApplyingDOT;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        timer = DOTInterval;
        interactable = other.collider.GetComponent<Interactable>();
        interactable.Interact();
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (!applyDOTOnStay)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            interactable.Interact();
            timer = DOTInterval;
        }
    }
}
