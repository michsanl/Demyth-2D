using CustomTools.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demyth.Gameplay
{
    public class Gate : Interactable
    {
        public Vector3 EnterPoint => transform.position;
        [SerializeField, ValueDropdown(nameof(LevelName))]
        private string targetLevel;
        private Level _level;

        public void SetupGate(Level level)
        {
            _level = level;
        }

        public override void Interact(Vector3 direction = default)
        {
            _level.MoveToNextLevel(targetLevel);
        }

#if UNITY_EDITOR
        private IEnumerable<string> LevelName()
        {
            string levelPath = "Assets/@Productions/Prefabs/Level Map";
            string[] guids = UnityEditor.AssetDatabase.FindAssets("", new[] { levelPath });
            return guids
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(y => UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(y).name);
        }
#endif
    }
}