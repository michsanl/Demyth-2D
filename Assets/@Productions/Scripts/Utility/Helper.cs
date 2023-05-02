using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// non allocating WaitForSeconds, by Tarodev

public static class Helper
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static bool CheckTargetDirection<T>(Vector2 origin, Vector2 dir, LayerMask layer, out T targetComponent)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, 1f, layer);
        
        targetComponent = default(T);
        if (hit)
            targetComponent = hit.collider.GetComponent<T>();

        return hit;
    }

    public static void MoveToPosition(Transform transform, Vector3 target, float duration)
    {
        transform.DOMove(target, duration).SetEase(Ease.OutExpo);
    }
}
