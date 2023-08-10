using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {
    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayerMask;

    public event Action OnSelectedUnitAction;

    private Unit selectedUnit;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        if (HandleSelectUnit()) {
            return;
        }

        HandleMoveUnit();
    }

    private bool HandleSelectUnit() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hitSuccess = Physics.Raycast(ray, out var hitInfo, float.MaxValue, unitsLayerMask);
        if (!hitSuccess) {
            return false;
        }

        var unit = hitInfo.collider.GetComponent<Unit>();
        SetSelectedUnit(unit);

        return true;
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        OnSelectedUnitAction?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    private void HandleMoveUnit() {
        if (selectedUnit == null) {
            return;
        }

        var worldPos = MouseWorld.GetPosition();
        var gridPos = LevelGrid.Instance.GetGridPosition(worldPos);
        worldPos = LevelGrid.Instance.GetWorldPosition(gridPos);
        selectedUnit.GetMoveAction().Move(worldPos);
    }
}