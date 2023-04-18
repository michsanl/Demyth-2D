using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : Interactable
{
    [SerializeField] private TemporarySaveDataSO temporarySaveDataSO;

    public override void ChangeLevel() 
    {
        if (temporarySaveDataSO.level01.isNextLevelUnlocked == true) 
        {
            GameManager.Instance.SetState(GameState.ExitLevel);
            Debug.Log("Going to next level");
        } else 
        {
            Debug.Log("Cannot go to next level");
        }
    }
}
