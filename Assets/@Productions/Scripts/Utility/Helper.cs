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
	    var originWithOffset = origin + dir;

        RaycastHit2D[] hit = Physics2D.RaycastAll(originWithOffset, dir, .1f, layer);
        
        targetComponent = default(T);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                targetComponent = hit[i].collider.GetComponent<T>();
                if (targetComponent != null)
                    break;
            }
        }

        return hit.Length > 0;
    }

    public static bool CheckTargetDirection<T>(Vector2 origin, Vector2 dir, Vector2 size, LayerMask layer, out T targetComponent)
    {
        var originWithOffset = origin + dir;
        var shrinkSize = size * 0.9f;

        RaycastHit2D[] hit = GetRaycasthitBasedOnSize(originWithOffset, shrinkSize, dir, layer);

        targetComponent = default(T);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                targetComponent = hit[i].collider.GetComponent<T>();
                if (targetComponent != null)
                    break;
            }
        }

        return hit.Length > 0;
    }

    private static RaycastHit2D[] GetRaycasthitBasedOnSize(Vector2 origin, Vector2 size, Vector2 dir, LayerMask layer)
    {
        RaycastHit2D[] hit;

        if (dir.x != 0)
        {
            hit = size.y > 1 ? Physics2D.BoxCastAll(origin, new Vector2(.9f, size.y), 0f, dir.normalized, 0f, layer) : Physics2D.LinecastAll(origin, origin, layer);
        } else
        {
            hit = size.x > 1 ? Physics2D.BoxCastAll(origin, new Vector2(size.x, .9f), 0f, dir.normalized, 0f, layer) : Physics2D.LinecastAll(origin, origin, layer);
        }

        return hit;
    }

    public static void MoveToPosition(Transform transform, Vector3 target, float duration)
    {
        transform.DOMove(target, duration).SetEase(Ease.OutExpo);
    }
}
