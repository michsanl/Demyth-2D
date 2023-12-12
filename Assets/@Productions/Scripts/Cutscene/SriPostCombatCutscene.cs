using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using PixelCrushers;

public class SriPostCombatCutscene : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsToActivate;
    [SerializeField] private GameObject[] _gameObjectsToDeactivate;
    
    public void StartCutscene()
    {
        // SEQUENCE 1
        // Deactivate summoned object
        // Deactivate boss sri
        // Deactivate NPC on all previous floor
        // Activate end game NPC and portal
        // Save

        LeanPool.DespawnAll();

        foreach (var item in _gameObjectsToActivate)
        {
            item.SetActive(true);
        }

        foreach (var item in _gameObjectsToDeactivate)
        {
            item.SetActive(false);
        }

        SaveSystem.SaveToSlot(1);
    }
}
