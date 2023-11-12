using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Enum Id/New ID")]
    public class EnumId : ScriptableObject
    {
    }

    public static class UIEnumExtension
    {
        public static bool IsEqual(this EnumId origin, EnumId comparer)
        {
            if (origin == null || comparer == null)
                return false;

            return origin.name == comparer.name;
        }
    }
}