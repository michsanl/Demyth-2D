using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Demyth/Level")]
public class LevelDataSO : ScriptableObject
{
    public string ID => $"{LevelName}-demyth";
    public string LevelName;
    public GameObject LevelModel;
}
