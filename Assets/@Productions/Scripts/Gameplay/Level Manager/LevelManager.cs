using CustomExtensions;
using CustomTools.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class LevelManager : SceneService
{
    public Level CurrentLevel { get; private set; }
    public Level MainMenuLevel => mainMenuLevel;
    public Action OnOpenMainMenu;
    public Action OnOpenGameLevel;

    [SerializeField] private Level mainMenuLevel;
    [SerializeField] private List<Level> levels = new List<Level>();
    
    protected override void OnInitialize()
    {
        GetLevelOnChild();
        SetLevelContext();

        SetLevel(mainMenuLevel);
        CurrentLevel = mainMenuLevel;
    }

    public void SetLevel(Level targetLevel)
    {
        foreach (var level in levels)
        {
            level.SetActive(level == targetLevel);            
        }
        CurrentLevel = targetLevel;

        if (targetLevel == mainMenuLevel)
        {
            OnOpenMainMenu?.Invoke();
        }
        else
        {
            OnOpenGameLevel?.Invoke();
            SetPlayerPosition(targetLevel.StarterPosition);
        }
    }

    public void ChangeLevelByGate(string previousLevelID, string nextLevelID)
    {
        var nextLevel = GetLevelByID(nextLevelID);
        var previousLevel = GetLevelByID(previousLevelID);
        
        var previousLevelPoint = nextLevel.GetLevelPoint(previousLevelID);
        SetPlayerPosition(previousLevelPoint);

        previousLevel.SetActive(false);
        nextLevel.SetActive(true);

        CurrentLevel = nextLevel;
    }

    public Level GetLevelByID(string id)
    {
        return levels.FirstOrDefault(x => x.ID == id);
    }

    private void SetPlayerPosition(Vector3 targetPosition)
    {
        Context.Player.transform.position = targetPosition;
    }

    private void SetLevelContext()
    {
        foreach (var level in levels)
        {
            level.Context = Context;
        }
    }

#if UNITY_EDITOR
    #region EDITOR HELPER
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
    #endregion
#endif
}