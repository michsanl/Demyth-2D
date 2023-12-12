using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class SriPostCombatCutscene : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsToActivate;
    [SerializeField] private GameObject[] _gameObjectsToDeactivate;
    
    public void StartCutscene()
    {
        // SEQUENCE 1
        // Deactivae summoned object
        // Deactivate boss sri
        // Activate npc

        LeanPool.DespawnAll();

        foreach (var item in _gameObjectsToActivate)
        {
            item.SetActive(true);
        }

        foreach (var item in _gameObjectsToDeactivate)
        {
            item.SetActive(false);
        }
    }
}
