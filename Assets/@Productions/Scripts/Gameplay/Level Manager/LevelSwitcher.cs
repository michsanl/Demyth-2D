using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demyth.Gameplay
{
    public class LevelSwitcher : MonoBehaviour
    {
        private LevelManager levelManager;

        private void Awake()
        {
            levelManager = GetComponentInParent<LevelManager>();
        }

        public void Interact(Vector3 dir = default)
        {
            
        }
    }
}