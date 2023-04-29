using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Level[] levels;


    [Button]
    private void GetLevelOnChild()
    {
        levels = GetComponentsInChildren<Level>(true);
    }
}