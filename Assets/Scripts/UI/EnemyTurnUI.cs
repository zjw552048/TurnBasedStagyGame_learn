using UnityEngine;

public class EnemyTurnUI : MonoBehaviour {
    private void Start() {
        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;

        UpdateVisual();
    }

    private void TurnManager_OnTurnChangedAction() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        if (TurnManager.Instance.IsPlayerTurn()) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}