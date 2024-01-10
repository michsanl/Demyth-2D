using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Settings SO/New Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Ara Parameter")]
    public bool UsePanOnStart;
    public bool UnlockLanternOnStart;
    public bool UnlockPotionOnStart;
    public bool UnlockShieldOnStart;
    
    [Header("Level Parameter")]
    public bool UnlockAllGateOnStart;
    
    [Header("Main Menu UI Parameter")]
    public bool ShowLevelSelect;
}
