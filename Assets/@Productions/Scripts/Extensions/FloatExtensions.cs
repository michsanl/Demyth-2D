using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static float CalculateCriticalDamage(this float baseDamage, float critRate)
    {
        float damage = baseDamage + (baseDamage * critRate / 100);
        return damage;
    }

    public static bool CalculateCriticalChance(this float chance)
    {
        return chance < Random.Range(0, 100);
    }

    public static bool IsCritical(this float critRate)
    {
        //Do not let critRate more than 100%
        critRate = Mathf.Clamp(critRate, 0, 100);
        return Random.Range(0, 100) <= critRate;
    }
}
