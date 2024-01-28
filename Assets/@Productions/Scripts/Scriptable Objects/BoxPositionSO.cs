using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Box Position/Box Puzzle Reset Position")]
public class BoxPositionSO : ScriptableObject
{
    [Header("Level 3")]
    public Vector3 PlayerResetPosition;
    public Vector3[] BoxCrateResetPositionArray;
    public Vector3[] BoxCardBoardOpenResetPositionArray;
    public Vector3[] BoxCardboardClosedResetPositionArray;
    [Header("Level 5")]
    public Vector2 YulaPosition;
    public Vector2 YuliPosition;
    public Vector2[] BoxWoodPositionArray;
}
