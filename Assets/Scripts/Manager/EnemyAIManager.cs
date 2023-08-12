using System;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour {
    private enum State {
        WaitEnemyAITurn,
        TakingAction,
        Busy,
    }

    private State state;
    private float takeActionTimer;

    private void Awake() {
        state = State.WaitEnemyAITurn;
    }

    private void Start() {
        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;
    }

    private void TurnManager_OnTurnChangedAction() {
        if (TurnManager.Instance.IsPlayerTurn()) {
            return;
        }

        state = State.TakingAction;
        takeActionTimer = 0.5f;
    }

    private void Update() {
        switch (state) {
            case State.WaitEnemyAITurn:
                break;

            case State.TakingAction:
                takeActionTimer -= Time.deltaTime;
                if (takeActionTimer <= 0) {
                    // 尝试找到一个EnemyUnit并执行一个行为
                    if (TryEnemyUnitTakeAction(ActionCompleted)) {
                        // 如果找到，则设置Busy
                        state = State.Busy;
                    } else {
                        // 如果未找到，则EnemyTurn结束
                        state = State.WaitEnemyAITurn;
                        TurnManager.Instance.EnterNextTurn();
                        Debug.Log("Enemy Turn Over!");
                    }
                }

                break;

            case State.Busy:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ActionCompleted() {
        state = State.TakingAction;
        takeActionTimer = 0.5f;
    }

    private bool TryEnemyUnitTakeAction(Action actionCompletedCallback) {
        var enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
        // 遍历EnemyUnit尝试执行一个行为
        foreach (var enemyUnit in enemyUnitList) {
            if (TryTakeAction(enemyUnit, actionCompletedCallback)) {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeAction(Unit selectedUnit, Action actionCompletedCallback) {
        // 找到Unit优先级最高的Action
        BaseAction bestAction = null;
        EnemyAIAction bestEnemyAIAction = default;
        foreach (var baseAction in selectedUnit.GetBaseActions()) {
            var testEnemyAIAction = baseAction.GetBestEnemyAIAction();
            if (bestAction == null ||
                bestEnemyAIAction.actionPriority < testEnemyAIAction.actionPriority) {
                bestAction = baseAction;
                bestEnemyAIAction = testEnemyAIAction;
            }
        }

        if (bestAction == null) {
            return false;
        }

        var gridPos = bestEnemyAIAction.gridPosition;

        if (!bestAction.IsValidActionGridPosition(gridPos)) {
            return false;
        }

        if (!selectedUnit.TrySpendActionPointsToTakeAction(bestAction)) {
            return false;
        }

        bestAction.TakeAction(gridPos, actionCompletedCallback);
        return true;
    }
}