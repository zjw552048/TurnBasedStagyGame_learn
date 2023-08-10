using UnityEngine;

public class UnitActionUI : MonoBehaviour {
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitAction += UnitActionManager_OnSelectedUnitAction;
        CreateActionButtonUI();
    }

    private void UnitActionManager_OnSelectedUnitAction() {
        CreateActionButtonUI();
    }

    private void CreateActionButtonUI() {
        foreach (Transform childTransform in actionButtonContainerTransform) {
            Destroy(childTransform.gameObject);
        }

        var selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        if (selectedUnit == null) {
            return;
        }

        var baseActionArray = selectedUnit.GetBaseActions();
        foreach (var baseAction in baseActionArray) {
            var actionButtonUiTransform = Instantiate(actionButtonUIPrefab, actionButtonContainerTransform);
            var actionButtonUI = actionButtonUiTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }
}