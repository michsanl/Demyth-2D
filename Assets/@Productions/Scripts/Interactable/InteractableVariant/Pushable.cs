using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pushable : Interactable
{
    public override void Interact(Vector3 direction)
    {
        Vector3 targetLocation = transform.position + direction;
        float pushDuration = .2f;

        transform.DOMove(targetLocation, pushDuration).SetEase(Ease.OutExpo);
        Debug.Log("Kicking box");
    }
}
