using CustomExtensions;
using CustomTools.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : SceneService
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Transform _player;

    public IEnumerator SetupLevel()
    {
        yield return new WaitForEndOfFrame();
    }

    public void ChangeLevel(Level levelFrom, Level levelTo)
    {
        var setting = levelTo.LevelSetting.FirstOrDefault(x => x.OtherLevel == levelFrom);
        if (setting.EnterPoint == null) return;

        MovePlayerToPosition(setting.EnterPoint.position);

        levelFrom.gameObject.SetActive(false);
        levelTo.gameObject.SetActive(true);
    }

    public void SetLevel(Level level)
    {
        foreach (var mapLevel in levels)
        {
            mapLevel.SetActive(mapLevel == level);            
        }

        if (level == null) return;

        level.SetActive(true);
        MovePlayerToPosition(level.StarterPosition);
    }

    private void MovePlayerToPosition(Vector3 point)
    {
        if (_player == null) return;
        _player.position = point;
    }

#if UNITY_EDITOR
    #region EDITOR HELPER
    [Title("Editor Helper")]
    [SerializeField, ValueDropdown("LevelList")]
    private string debugLevelID;
    private IEnumerable<string> LevelList => levels.Select(x => x.LevelName);
    [Button]
    private void DebugOpenLevel()
    {
        var level = levels.FirstOrDefault(x => x.LevelName == debugLevelID);
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