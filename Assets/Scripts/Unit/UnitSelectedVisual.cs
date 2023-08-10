using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitAction += UnitActionManager_OnSelectedUnitAction;

        UpdateVisual();
    }

    private void UnitActionManager_OnSelectedUnitAction() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        var selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        meshRenderer.enabled = selectedUnit == unit;
    }
}