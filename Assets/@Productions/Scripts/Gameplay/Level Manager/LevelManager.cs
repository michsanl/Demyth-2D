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
    public Action OnLevelChanged;
    public Level CurrentLevel { get; private set; }

    [SerializeField]
    private List<Level> levels = new List<Level>();

    protected override void OnInitialize()
    {
        GetLevelOnChild();
        foreach (var level in levels)
        {
            level.Context = Context;
        }
        CurrentLevel = GetLevelByID(debugLevelID);
    }

    public IEnumerator SetupLevel()
    {
        yield return new WaitForEndOfFrame();
    }

    public void SetLevel(Level level)
    {
        foreach (var mapLevel in levels)
        {
            mapLevel.SetActive(mapLevel == level);            
        }

        if (level == null) return;

        level.SetActive(true);
    }

    public Level GetLevelByID(string id)
    {
        return levels.FirstOrDefault(x => x.ID == id);
    }

    public void ChangeLevel(string previousLevelID, string nextLevelID)
    {
        var nextLevel = GetLevelByID(nextLevelID);
        var previousLevel = GetLevelByID(previousLevelID);
        
        var previousLevelPoint = nextLevel.GetLevelPoint(previousLevelID);
        SetPlayerPosition(previousLevelPoint);

        previousLevel.SetActive(false);
        nextLevel.SetActive(true);
        CurrentLevel = nextLevel;
    }

    public void ChangeLevel(Level targetLevel)
    {
        foreach (var mapLevel in levels)
        {
            mapLevel.SetActive(mapLevel == targetLevel);            
        }
        
        targetLevel.SetActive(true);
        CurrentLevel = targetLevel;
        OnLevelChanged?.Invoke();
    }

    private void SetPlayerPosition(Vector3 levelStarterPosition)
    {
        Context.Player.transform.position = levelStarterPosition;
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
        var level = levels.FirstOrDefault(x => x.ID == debugLevelID);
        if (level == null) return;

        SetLevel(level);
    }

    [Button]
    private void GetLevelOnChild()
    {
        levels = GetComponentsInChildren<Level>(true).ToList();
    }
    #endregion
#endif
}