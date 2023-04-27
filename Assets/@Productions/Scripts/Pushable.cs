using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pushable : Interactable
{
    public override void Push(Vector3 direction, float pushDuration)
    {
        Vector3 finalDirection = transform.position + direction;
        Debug.Log("Kicking box");
        transform.DOMove(finalDirection, pushDuration).SetEase(Ease.OutExpo);
    }
}
