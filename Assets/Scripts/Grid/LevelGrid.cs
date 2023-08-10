using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform debugGridPrefab;

    private GridObjectManager gridObjectManager;

    private void Awake() {
        gridObjectManager = new GridObjectManager(10, 10, 2);
        gridObjectManager.CreateDebugGrids(transform, debugGridPrefab);
        
        Instance = this;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log($"LevelGrid click GridPosition: {gridObjectManager.GetGridPosition(MouseWorld.GetPosition())}");
        }
    }

    #region Grid基本信息

    public int GetWidth() {
        return gridObjectManager.GetWidth();
    }

    public int GetHeight() {
        return gridObjectManager.GetHeight();
    }

    public float GetCellSize() {
        return gridObjectManager.GetCellSize();
    }

    #endregion

    #region Unit、Grid映射

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
        var gridObject = gridObjectManager.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition) {
        var gridObject = gridObjectManager.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition) {
        var gridObject = gridObjectManager.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void MoveUnitGridPosition(Unit unit, GridPosition fromPos, GridPosition toPos) {
        RemoveUnitAtGridPosition(unit, fromPos);
        AddUnitAtGridPosition(unit, toPos);
    }

    public bool HasUnitAtGridPosition(GridPosition gridPosition) {
        var gridObject = gridObjectManager.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    #endregion

    #region 坐标转换

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return gridObjectManager.GetWorldPosition(gridPosition);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return gridObjectManager.GetGridPosition(worldPosition);
    }

    #endregion

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridObjectManager.IsValidGridPosition(gridPosition);
    }
}