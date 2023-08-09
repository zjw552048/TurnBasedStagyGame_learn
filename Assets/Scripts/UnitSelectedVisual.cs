using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void Start() {
        
        UnitActionSystem.Instance.OnSelectedUnitAction += UnitActionSystem_OnSelectedUnitAction;
    }

    private void UnitActionSystem_OnSelectedUnitAction() {
        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        meshRenderer.enabled = selectedUnit == unit;
    }
}
