using System.Collections;
using System;
using UnityEngine;
using CodeMonkey.Utils;

namespace CustomCode.GridClass3D
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
            public int z;
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
                for (int z = 0; z < height; z++)
                {
                    gridArray[x, z] = createGridObject(this, x, z);
                }
            }

            showDebug = false;
            if (showDebug)
            {
                TextMesh[,] textArray = new TextMesh[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        textArray[i, j] = UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null, 
                            GetWorldPosition(i, j) + new Vector3(cellSize, 0, cellSize) * 0.5f, 20, Color.white, 
                            TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridValueChange += (object sender, OnGridValueChangeEventArgs e) =>
                {
                    textArray[e.x, e.z].text = gridArray[e.x, e.z].ToString();
                };
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
        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize + originPosition;
        }

        //Mengubah nilai world position ke posisi dalam grid
        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        }
        #endregion

        #region SET GRID VALUE
        public void SetGridObject(int x, int z, TGridObject value)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                gridArray[x, z] = value;
                if (OnGridValueChange != null)
                    OnGridValueChange(this, new OnGridValueChangeEventArgs { x = x, z = z });
            }
        }

        public void TriggerChangeEvent(int x, int z)
        {
            if (OnGridValueChange != null)
                OnGridValueChange(this, new OnGridValueChangeEventArgs { x = x, z = z });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            SetGridObject(x, z, value);
        }
        #endregion

        #region GET GRID VALUE
        public TGridObject GetGridObject(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < width && z < height)
            {
                return gridArray[x, z];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }
        #endregion
    }

}