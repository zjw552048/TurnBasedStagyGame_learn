using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionManager : MonoBehaviour {
    public static UnitActionManager Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayerMask;

    public event Action OnSelectedUnitAction;
    public event Action OnSelectedActionAction;
    public event Action OnSelectedActionStartedAction;
    
    public event Action<bool> OnUnitActionBusyChangedAction;

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
            if (EventSystem.current.IsPointerOverGameObject()) {
                // 点击到UI
                return;
            }

            if (HandleSelectUnit()) {
                // 点击到Unit
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
        if (selectedUnit == unit) {
            return false;
        }

        SetSelectedUnit(unit);

        return true;
    }

    private void HandleSelectedAction() {
        if (selectedUnit == null) {
            return;
        }

        if (selectedAction == null) {
            return;
        }

        var worldPos = MouseWorld.GetPosition();
        var gridPos = LevelGrid.Instance.GetGridPosition(worldPos);

        if (!selectedAction.IsValidMoveActionGridPosition(gridPos)) {
            return;
        }

        if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) {
            return;
        }

        SetBusy();
        selectedAction.TakeAction(gridPos, ClearBusy);
        OnSelectedActionStartedAction?.Invoke();
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        OnSelectedUnitAction?.Invoke();
        
        SetSelectedAction(unit.GetDefaultAction());
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        selectedAction = baseAction;
        OnSelectedActionAction?.Invoke();
    }

    public BaseAction GetSelectedAction() {
        return selectedAction;
    }

    #region busying状态

    private void SetBusy() {
        busying = true;
        OnUnitActionBusyChangedAction?.Invoke(busying);
    }

    private void ClearBusy() {
        busying = false;
        OnUnitActionBusyChangedAction?.Invoke(busying);
    }

    #endregion
}