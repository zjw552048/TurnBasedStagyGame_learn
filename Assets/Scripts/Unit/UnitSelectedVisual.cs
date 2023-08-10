using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitAction += UnitActionSystem_OnSelectedUnitAction;

        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitAction() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        meshRenderer.enabled = selectedUnit == unit;
    }
}