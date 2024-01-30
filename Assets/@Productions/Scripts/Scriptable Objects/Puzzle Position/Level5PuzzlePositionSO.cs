using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle Level Position/new Level 5 Position")]
public class Level5PuzzlePositionSO : ScriptableObject
{
    public Vector2 PlayerPosition;
    public Vector2 YulaPosition;
    public Vector2 YuliPosition;
    public Vector2[] BoxPositions;
}
