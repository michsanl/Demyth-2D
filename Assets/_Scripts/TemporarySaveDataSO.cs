using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
public class TemporarySaveDataSO : ScriptableObject
{
    public Level01 level01;
    public bool isNextLevelUnlocked;
}

[Serializable]
public struct Level01 
{
    public bool isNextLevelUnlocked;
    public bool isNPCDialogueTriggered;
    public Vector3 playerSpawnPosition;
    public Vector3 playerDirection;

}