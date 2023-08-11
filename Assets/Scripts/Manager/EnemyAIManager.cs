using UnityEngine;

public class EnemyAIManager : MonoBehaviour {
    private float enemyTurnTimer;

    private void Start() {
        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;
    }

    private void TurnManager_OnTurnChangedAction() {
        if (TurnManager.Instance.IsPlayerTurn()) {
            return;
        }

        enemyTurnTimer = 2f;
    }

    private void Update() {
        if (TurnManager.Instance.IsPlayerTurn()) {
            return;
        }

        enemyTurnTimer -= Time.deltaTime;
        if (enemyTurnTimer > 0) {
            return;
        }

        TurnManager.Instance.EnterNextTurn();
        Debug.Log("Enemy Turn Over!");
    }
}