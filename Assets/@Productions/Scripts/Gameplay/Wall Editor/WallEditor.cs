using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallSize
{
    [HorizontalGroup("Sizes"), OnValueChanged(nameof(EditSize))]
    public int size;
    [HorizontalGroup("Sizes"), HideLabel, OnValueChanged(nameof(EditSize))]
    public int width;
    public BoxCollider2D collider;
    public bool isVertical;

    private void EditSize()
    {
        if(isVertical)
            collider.size = new Vector2(width, size);
        else
            collider.size = new Vector2(size, width);
    }
}

public class WallEditor : MonoBehaviour
{
    [SerializeField, OnValueChanged(nameof(EditBorder))]
    private int width = 5;
    [SerializeField, OnValueChanged(nameof(EditBorder))]
    private int height = 5;

    [SerializeField, FoldoutGroup("Top Wall", expanded: true), HideLabel]
    private WallSize topWall;
    [Space(10)]
    [SerializeField, FoldoutGroup("Bottom Wall", expanded: true), HideLabel]
    private WallSize bottomWall;
    [Space(10)]
    [SerializeField, FoldoutGroup("Left Wall", expanded: true), HideLabel]
    private WallSize leftWall;
    [Space(10)]
    [SerializeField, FoldoutGroup("Right Wall", expanded: true), HideLabel]
    private WallSize rightWall;

    private void EditBorder()
    {
        topWall.collider.offset = new Vector2(0, height);
        bottomWall.collider.offset = new Vector2(0, -height);

        leftWall.collider.offset = new Vector2(-width, 0);
        rightWall.collider.offset = new Vector2(width, 0);
    }
}
