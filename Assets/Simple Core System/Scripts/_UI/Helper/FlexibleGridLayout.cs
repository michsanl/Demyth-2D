using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    #region GAME DEV GUIDE
    public enum FitType
    {
        Uniform, FixedRows, FixedColumns
    }

    public enum FitMode
    {
        Unconstrained,
        FlexibleWidth,
        FlexibleHeight
    }

    [Header("Grid Size")]
    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX, fitY;

    [Header("Size Fitter")]
    [SerializeField] protected FitMode horizontalFit = FitMode.Unconstrained;
    [SerializeField] protected FitMode verticalFit = FitMode.Unconstrained;

    public override void CalculateLayoutInputVertical()
    {

        if (fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;

            horizontalFit = FitMode.Unconstrained;
            verticalFit = FitMode.Unconstrained;

            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        else if (fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        if(fitType != FitType.Uniform)
        {
            if(fitX && fitY)
            {
                horizontalFit = FitMode.Unconstrained;
                verticalFit = FitMode.Unconstrained;
            }
            else if (fitX)
            {
                horizontalFit = FitMode.Unconstrained;
                verticalFit = FitMode.FlexibleHeight;
            }
            else if (fitY)
            {
                horizontalFit = FitMode.FlexibleWidth;
                verticalFit = FitMode.Unconstrained;
            }
        }

        float parentHeight = rectTransform.rect.height;
        float parentWidth = rectTransform.rect.width;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int rowCount = 0;
        int columnCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
        HandleSelfFittingAlongAxis(0);
    }

    public override void SetLayoutVertical()
    {
        HandleSelfFittingAlongAxis(1);
    }

    private void HandleSelfFittingAlongAxis(int axis)
    {
        FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
        if (fitting == FitMode.Unconstrained)
        {
            // Keep a reference to the tracked transform, but don't control its properties:
            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
            return;
        }

        m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));
        float size = axis == 0 ? 
            columns * cellSize.x : 
            rows * cellSize.y + (padding.top + padding.bottom + (spacing.y * (rows - 1)));
        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
    }
    #endregion
}
