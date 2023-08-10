using System;
using UnityEngine;

public class UnitActionManager : MonoBehaviour {
    public static UnitActionManager Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayerMask;

    public event Action OnSelectedUnitAction;

    private Unit selectedUnit;

    private bool busying;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (busying) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (HandleSelectUnit()) {
                return;
            }

            HandleMoveUnit();
        }

        if (Input.GetMouseButtonDown(1)) {
            HandleSpinUnit();
        }
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

    #region busying状态

    private void SetBusy() {
        busying = true;
    }

    private void ClearBusy() {
        busying = false;
    }

    #endregion

    private void HandleMoveUnit() {
        if (selectedUnit == null) {
            return;
        }

        var worldPos = MouseWorld.GetPosition();
        var gridPos = LevelGrid.Instance.GetGridPosition(worldPos);
        var moveAction = selectedUnit.GetMoveAction();
        if (!moveAction.IsValidMoveActionGridPosition(gridPos)) {
            return;
        }

        SetBusy();
        moveAction.Move(gridPos, ClearBusy);
    }

    private void HandleSpinUnit() {
        if (selectedUnit == null) {
            return;
        }

        var spineAction = selectedUnit.GetSpineAction();
        
        SetBusy();
        spineAction.Spin(ClearBusy);
    }
}