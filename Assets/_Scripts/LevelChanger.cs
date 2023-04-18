using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : Interactable
{
    [SerializeField] private TemporarySaveDataSO temporarySaveDataSO;

    public override void Interact() {
        if (temporarySaveDataSO.level01.isNextLevelUnlocked == true) {
            GameManager.Instance.SetState(FirstLevelState.ExitLevel);
            Debug.Log("Going to next level");
        } else {
            Debug.Log("Cannot go to next level");
        }
    }

    public override void Push(Vector3 direction, float moveDuration)
    {
    }
}
