using UnityEngine;

public class UnitActionBusyUI : MonoBehaviour {
    private void Start() {
        UnitActionManager.Instance.OnUnitActionBusyChangedAction += UnitActionManager_OnUnitActionBusyChangedAction;

        Hide();
    }

    private void UnitActionManager_OnUnitActionBusyChangedAction(bool busying) {
        if (busying) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}