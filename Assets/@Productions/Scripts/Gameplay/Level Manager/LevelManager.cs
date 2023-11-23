using CustomExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Core;
using UnityEngine.Events;

public class LevelManager : SceneService
{
    public Level CurrentLevel { get; private set; }

    [SerializeField] 
    private EnumId starterLevel;
    [SerializeField] 
    private Dictionary<EnumId, Level> levels = new();

    public UnityEvent OnOpenMainMenu;
    public UnityEvent OnOpenGameLevel;

    private void Awake()
    {
        SetLevelContext();
        OpenLevel(starterLevel);
    }

    public void OpenLevel(EnumId targetLevelId)
    {
        foreach (var lvl in levels.Values)
        {
            lvl.SetActive(lvl.ID == targetLevelId);
        }

        var level = GetLevelByID(targetLevelId);
        SetLevel(level);
    }

    public void SetLevel(Level targetLevel)
    {
        CurrentLevel = targetLevel;

        if (targetLevel == starterLevel)
        {
            OnOpenMainMenu?.Invoke();
        }
        else
        {
            OnOpenGameLevel?.Invoke();
            SetPlayerPosition(targetLevel.StarterPosition);
        }
    }

    /*public void ChangeLevelByGate(string previousLevelID, string nextLevelID)
    {
        var nextLevel = GetLevelByID(nextLevelID);
        var previousLevel = GetLevelByID(previousLevelID);
        
        var previousLevelPoint = nextLevel.GetLevelPoint(previousLevelID);
        SetPlayerPosition(previousLevelPoint);

        previousLevel.SetActive(false);
        nextLevel.SetActive(true);

        CurrentLevel = nextLevel;
    }*/

    public Level GetLevelByID(EnumId levelId)
    {
        return levels[levelId];
    }

    private void SetPlayerPosition(Vector3 targetPosition)
    {
        //Context.Player.transform.position = targetPosition;
    }

    private void SetLevelContext()
    {
        foreach (var level in levels)
        {
            //level.Context = Context;
        }
    }

#if UNITY_EDITOR
    /*#region EDITOR HELPER
    [Title("Editor Helper")]
    [SerializeField, ValueDropdown("LevelList")]
    private string debugLevelID;
    private IEnumerable<string> LevelList => levels.Select(x => x.ID);
    [Button]
    private void DebugOpenLevel()
    {
        var targetLevel = levels.FirstOrDefault(x => x.ID == debugLevelID);
        if (targetLevel == null) return;
        
        foreach (var level in levels)
        {
            level.SetActive(level == targetLevel);            
        }
    }

    [Button]
    private void GetLevelOnChild()
    {
        levels = GetComponentsInChildren<Level>(true).ToList();
    }
    #endregion*/
#endif
}
