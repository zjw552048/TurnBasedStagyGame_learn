using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform debugGridPrefab;

    private GridSystem gridSystem;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugGrid(debugGridPrefab);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log($"LevelGrid click GridPosition: {gridSystem.GetGridPosition(MouseWorld.GetPosition())}");
        }
    }

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

    #endregion

    #region 坐标转换

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return gridSystem.GetGridPosition(worldPosition);
    }

    #endregion
}