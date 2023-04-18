using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pushable : Interactable
{
    public override void Push(Vector3 direction, float duration)
    {
        Vector3 finalDirection = transform.position + direction;
        Debug.Log("Kicking box");
        transform.DOMove(finalDirection, duration).SetEase(Ease.OutExpo);
    }
}
