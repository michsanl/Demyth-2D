using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(menuName = "Box Position/Box Puzzle Reset Position")]
public class BoxPositionSO : ScriptableObject
{
    
    public Vector3 PlayerResetPosition;
    public Vector3[] BoxCrateResetPositionArray;
    public Vector3[] BoxCardBoardOpenResetPositionArray;
    public Vector3[] BoxCardboardClosedResetPositionArray;

}
