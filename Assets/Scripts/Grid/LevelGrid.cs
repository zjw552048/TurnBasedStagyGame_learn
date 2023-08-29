using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform debugGridPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    private void Awake() {
        gridSystem = new GridSystem<GridObject>(
            width,
            height,
            cellSize,
            gridPosition => new GridObject(gridPosition));
        
        // 注释debugGrid
        // gridSystem.CreateDebugGrids(transform, debugGridPrefab);

        Instance = this;
    }

    private void Start() {
        PathfindingManager.Instance.SetUp(width, height, cellSize);
    }

    #region Grid基本信息

    public int GetWidth() {
        return gridSystem.GetWidth();
    }

    public int GetHeight() {
        return gridSystem.GetHeight();
    }

    public float GetCellSize() {
        return gridSystem.GetCellSize();
    }

    #endregion

    #region Unit、Grid映射

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void MoveUnitGridPosition(Unit unit, GridPosition fromPos, GridPosition toPos) {
        RemoveUnitAtGridPosition(unit, fromPos);
        AddUnitAtGridPosition(unit, toPos);
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }
    
    public void SetInteractableAtGridPosition(IInteractable interactable, GridPosition gridPosition) {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

    #endregion

    #region 坐标转换

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return gridSystem.GetGridPosition(worldPosition);
    }

    #endregion

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridSystem.IsValidGridPosition(gridPosition);
    }
}