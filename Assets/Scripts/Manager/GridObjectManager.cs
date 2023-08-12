using System;
using UnityEngine;

public class GridObjectManager<TGridObject> {
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly TGridObject[,] gridObjectArray;

    public GridObjectManager(int width, int height, float cellSize, Func<GridPosition, TGridObject> createTGridObjectFuc) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];

        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createTGridObjectFuc(gridPosition);
            }
        }
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridPosition.x >= 0 &&
               gridPosition.x < width &&
               gridPosition.z >= 0 &&
               gridPosition.z < height;
    }

    #region Grid\World position转换

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    #endregion


    #region GridObject

    public TGridObject GetGridObject(GridPosition gridPosition) {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    #endregion

    public void CreateDebugGrids(Transform parentTransform, Transform debugGridPrefab) {
        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridPosition = new GridPosition(x, z);
                var gridObject = GetGridObject(gridPosition);

                var debugGridTransform = GameObject.Instantiate(debugGridPrefab, parentTransform);
                debugGridTransform.position = GetWorldPosition(gridPosition);

                var debugGrid = debugGridTransform.GetComponent<DebugGridObject>();
                debugGrid.SetGridObject(gridObject as GridObject);
            }
        }
    }
}