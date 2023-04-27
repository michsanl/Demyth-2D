using System.Collections;
using System;
using UnityEngine;
using CodeMonkey.Utils;

namespace CustomCode.GridClass2D
{
    public class GridClass <TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;
        Vector3 originPosition;

        public event EventHandler<OnGridValueChangeEventArgs> OnGridValueChange;
        public class OnGridValueChangeEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private TGridObject[,] gridArray;

        private bool showDebug;

        public GridClass(int width, int height, float cellSize, Vector3 originPosition, Func<GridClass<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            showDebug = true;
            if (showDebug)
            {
                TextMesh[,] textArray = new TextMesh[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        /*textArray[i, j] = UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null,
                            GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, 15, Color.white,
                            TextAnchor.MiddleCenter);*/
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                /*OnGridValueChange += (object sender, OnGridValueChangeEventArgs e) =>
                {
                    textArray[e.x, e.y].text = gridArray[e.x, e.y].ToString();
                };*/
            }
        }

        public int GetHeight()
        {
            return height;
        }
        public int GetWidth()
        {
            return width;
        }
        public float GetCellSize()
        {
            return cellSize;
        }

        #region GRID POSITION
        //Mengambil world position dari masing-masing grid
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        //Mengubah nilai world position ke posisi dalam grid
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }
        #endregion

        #region SET GRID VALUE
        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (OnGridValueChange != null)
                    OnGridValueChange(this, new OnGridValueChangeEventArgs { x = x, y = y });
            }
        }

        public void TriggerChangeEvent(int x, int y)
        {
            if (OnGridValueChange != null)
                OnGridValueChange(this, new OnGridValueChangeEventArgs { x = x, y = y });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }
        #endregion

        #region GET GRID VALUE
        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
        #endregion
    }

}