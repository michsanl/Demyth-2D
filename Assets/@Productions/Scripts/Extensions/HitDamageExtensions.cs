using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomExtensions
{
    public static class HitDamageExtensions
    {
        public static float GetDamage(this float baseDamage, bool isCritical, float criticalDamage, float weakspotDamage, bool isWeakspot)
        {
            float critical = baseDamage.GetCriticalDamage(criticalDamage, isCritical);

            float weakspot = isWeakspot ? baseDamage.GetWeakspotDamage(weakspotDamage) : 0f;

            return baseDamage + critical + weakspot;
        }

        private static float GetCriticalDamage(this float baseDamage, float bonusCriticalDamage, bool isCritical)
        {
            float bonusCrit = isCritical ? baseDamage * (bonusCriticalDamage / 100f) : 0f;
            return bonusCrit;
        }
         
        private static float GetWeakspotDamage(this float baseDamage, float bonusWeakspotDamage)
        {
            float bonusWeakspot = baseDamage * (bonusWeakspotDamage / 100f);
            return bonusWeakspot;
        }

        public static bool IsCritical(this float criticalChance)
        {
            return Random.Range(0f, 100f) < criticalChance;
        }

        public static float CalculateResistence(this float damage, float resistenceAmount)
        {
            return damage * (1f - resistenceAmount / 100f);
        }
    }
}