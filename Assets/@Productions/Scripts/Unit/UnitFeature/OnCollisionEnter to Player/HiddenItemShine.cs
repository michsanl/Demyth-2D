using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiddenItemShine : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueTriggerGO;
    [SerializeField] private Animator _animator;

    private void OnCollisionEnter(Collision other) 
    {
        _animator.SetBool("Shine", true);
        _dialogueTriggerGO.SetActive(true);
    }

    private void OnCollisionExit(Collision other) 
    {
        _animator.SetBool("Shine", false);
        _dialogueTriggerGO.SetActive(false);
    }

}
