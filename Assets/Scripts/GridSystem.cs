using UnityEngine;

public class GridSystem {
    private Transform debugGridPrefab;
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly GridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugGrid(Transform debugGridPrefab) {
        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var gridPosition = new GridPosition(x, z);

                var worldPosition = GetWorldPosition(gridPosition);
                var debugGridTransform = GameObject.Instantiate(debugGridPrefab, worldPosition, Quaternion.identity);
                var debugGrid = debugGridTransform.GetComponent<DebugGrid>();
                debugGrid.SetText(gridPosition);
            }
        }
    }
}