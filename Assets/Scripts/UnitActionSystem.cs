using UnityEngine;

public class UnitActionSystem : MonoBehaviour {
    [SerializeField] private LayerMask unitsLayerMask;
    private Unit selectedUnit;

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
        selectedUnit = unit;

        return true;
    }

    private void HandleMoveUnit() {
        if (selectedUnit == null) {
            return;
        }

        selectedUnit.Move(MouseWorld.GetPosition());
    }
}