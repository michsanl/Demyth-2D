using Demyth.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelSetting
{
    public Level OtherLevel;
    public Transform EnterPoint;
}

public class Level : MonoBehaviour
{
    [SerializeField]
    private LevelSetting[] settings;

    [SerializeField]
    private Transform starterPoint;

    #region DEBUG HELPER
    private void OnDrawGizmos()
    {
        Color color = Color.white;
        color.a = 0.1f;
        Gizmos.color = color;
        DebugDrawLine();
    }
    private void DebugDrawLine()
    {
        int width = 17;
        int height = 10;
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