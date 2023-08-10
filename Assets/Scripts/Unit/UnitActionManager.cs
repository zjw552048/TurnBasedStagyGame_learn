using System;
using UnityEngine;

public class UnitActionManager : MonoBehaviour {
    public static UnitActionManager Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayerMask;

    public event Action OnSelectedUnitAction;

    private Unit selectedUnit;
    private BaseAction selectedAction;

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

            HandleSelectedAction();
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
        SetSelectedAction(unit.GetDefaultAction());
        OnSelectedUnitAction?.Invoke();
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        selectedAction = baseAction;
    }

    public BaseAction GetSelectedAction() {
        return selectedAction;
    }

    #region busying状态

    private void SetBusy() {
        busying = true;
    }

    private void ClearBusy() {
        busying = false;
    }

    #endregion

    private void HandleSelectedAction() {
        if (selectedUnit == null) {
            return;
        }

        switch (selectedAction) {
            case MoveAction moveAction:
                var worldPos = MouseWorld.GetPosition();
                var gridPos = LevelGrid.Instance.GetGridPosition(worldPos);
                if (!moveAction.IsValidMoveActionGridPosition(gridPos)) {
                    return;
                }

                SetBusy();
                moveAction.Move(gridPos, ClearBusy);
                break;

            case SpinAction spinAction:
                SetBusy();
                spinAction.Spin(ClearBusy);
                break;
        }
    }
}