using CustomTools.Core;
using Demyth.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LevelSetting
{
    [ValueDropdown(nameof(LevelName))]
    public string ID;
    public Gate Gate;

#if UNITY_EDITOR
    private IEnumerable<string> LevelName()
    {
        string levelPath = "Assets/@Productions/Prefabs/Level Map";
        string[] guids = UnityEditor.AssetDatabase.FindAssets("", new[] { levelPath });
        return guids
            .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
            .Select(y => UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(y).name);
    }
#endif
}

public class Level : CoreBehaviour
{
    public string ID => gameObject.name;
    public Vector3 StarterPosition => starterPoint.position;
    public LevelSetting[] LevelSetting => settings;

    [SerializeField]
    private LevelSetting[] settings;
    [SerializeField]
    private Transform starterPoint;

    private void Awake()
    {
        foreach (var setting in settings)
        {
            setting.Gate.SetupGate(this);
        }
    }

    public Vector2 GetLevelPoint(string levelID)
    {
        var setting = settings.FirstOrDefault(level => level.ID == levelID);
        return setting == null ? StarterPosition : setting.Gate.EnterPoint;
    }

    public void MoveToNextLevel(string levelID)
    {
        Context.LevelManager.ChangeLevelByGate(ID, levelID);
    }

    #region DEBUG HELPER
    private void OnDrawGizmos()
    {
        Color color = Color.white;
        color.a = 0.5f;
        Gizmos.color = color;
        DebugDrawLine();
    }
    private void DebugDrawLine()
    {
        int width = 17;
        int height = 30;
        Vector3 originGridPosition = transform.position + Vector3.up * 0.5f;
        Vector3 centerPosition = originGridPosition + new Vector3(-width / 2f, -height / 2f, 0);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawLine(GetWorldPosition(centerPosition, i, j), GetWorldPosition(centerPosition, i, j + 1));
                Gizmos.DrawLine(GetWorldPosition(centerPosition, i, j), GetWorldPosition(centerPosition, i + 1, j));
            }
        }
        Gizmos.DrawLine(GetWorldPosition(centerPosition, 0, height), GetWorldPosition(centerPosition, width, height));
        Gizmos.DrawLine(GetWorldPosition(centerPosition, width, 0), GetWorldPosition(centerPosition, width, height));
    }

    public Vector3 GetWorldPosition(Vector3 originPosition, int x, int z)
    {
        return new Vector3(x, z, 0) * 1f + originPosition;
    }
    #endregion
}