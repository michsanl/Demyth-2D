using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// non allocating WaitForSeconds semoga jadi ga bloodware, by Tarodev

public static class Helper
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }
}
