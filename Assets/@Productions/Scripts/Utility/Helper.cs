using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

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

    public static void PlayMusic(AudioClip audioClip, float volume, int id, bool loop)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = volume;
        playOptions.ID = id;
        playOptions.Loop = loop;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;

        MMSoundManagerSoundPlayEvent.Trigger(audioClip, playOptions);
    }

    public static AudioSource PlaySFX(AudioClip audioClip, float volume)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = volume;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        return MMSoundManagerSoundPlayEvent.Trigger(audioClip, playOptions);
    }

    public static AudioSource PlaySFXIgnorePause(AudioClip audioClip, float volume)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = volume;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;

        var source = MMSoundManagerSoundPlayEvent.Trigger(audioClip, playOptions);
        source.ignoreListenerPause = true;
        return source;
    }

    public static float GetAngleFromFectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static bool CheckTargetDirection<T>(Vector2 origin, Vector2 dir, LayerMask layer, out T targetComponent)
    {
	    var finalOrigin = origin + dir;

        // RaycastHit2D[] hit = Physics2D.RaycastAll(finalOrigin, dir, .1f, layer);
        RaycastHit2D[] hit = Physics2D.BoxCastAll(finalOrigin, new Vector2(0.9f, 0.9f), 0f, dir, 0f, layer);
        
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
    public static bool CheckTargetDirection<T>(Vector2 raycastOrigin, Vector2 dir, Vector2 objectSize, LayerMask layer, out T targetComponent)
    {
        var finalRaycastOrigin = raycastOrigin + dir;

        RaycastHit2D[] hit = GetRaycastHitScaleOnObjectSize(finalRaycastOrigin, objectSize, dir, layer);

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

    private static RaycastHit2D[] GetRaycastHitScaleOnObjectSize(Vector2 origin, Vector2 objectSize, Vector2 dir, LayerMask layer)
    {
        RaycastHit2D[] hit;
        
        float shrinkMultiplier = .9f;

        if (IsDirHorizontal(dir))
        {
            Vector2 boxCastSize = new Vector2(1f, objectSize.y) * shrinkMultiplier;
            hit = Physics2D.BoxCastAll(origin, boxCastSize, 0f, dir.normalized, 0f, layer);
        }
        else
        {
            Vector2 boxCastSize = new Vector2(objectSize.x, 1f) * shrinkMultiplier;
            hit = Physics2D.BoxCastAll(origin, boxCastSize, 0f, dir.normalized, 0f, layer);
        }

        return hit;
    }

    private static bool IsDirHorizontal(Vector2 dir)
    {
        return dir.x != 0;
    }

    public static void MoveToPosition(Transform transform, Vector3 target, float duration)
    {
        transform.DOMove(target, duration).SetEase(Ease.OutExpo);
    }
}
