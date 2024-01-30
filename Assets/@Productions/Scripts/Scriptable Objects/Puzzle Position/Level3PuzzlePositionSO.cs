using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle Level Position/new Level 3 Position")]
public class Level3PuzzlePositionSO : ScriptableObject
{
    public Vector2 PlayerPosition;
    public Vector3[] BoxCrateResetPositionArray;
    public Vector3[] BoxCardBoardOpenResetPositionArray;
    public Vector3[] BoxCardboardClosedResetPositionArray;
}
