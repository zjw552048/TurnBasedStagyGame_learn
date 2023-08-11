using System;
using UnityEngine;

public class Unit : MonoBehaviour {
    public static event Action OnAnyUnitActionPointsChangedAction;

    private GridPosition gridPosition;

    private BaseAction[] baseActionArray;

    private int actionPoints;
    private const int MAX_ACTION_POINTS = 2;

    private void Awake() {
        baseActionArray = GetComponents<BaseAction>();

        ResetActionPoints();
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);

        TurnManager.Instance.OnTurnChangedAction += TurnManager_OnTurnChangedAction;
    }

    private void TurnManager_OnTurnChangedAction() {
        ResetActionPoints();
    }

    private void Update() {
        HandleGridRefresh();
    }

    private void HandleGridRefresh() {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition == gridPosition) {
            return;
        }

        LevelGrid.Instance.MoveUnitGridPosition(this, gridPosition, newGridPosition);
        gridPosition = newGridPosition;
    }

    #region 获取unit信息

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public BaseAction GetDefaultAction() {
        return baseActionArray[0];
    }

    public BaseAction[] GetBaseActions() {
        return baseActionArray;
    }

    #endregion

    #region actionPoints

    private void ResetActionPoints() {
        actionPoints = MAX_ACTION_POINTS;
        OnAnyUnitActionPointsChangedAction?.Invoke();
    }

    public int GetActionPoints() {
        return actionPoints;
    }

    private void SpendActionPointsToTakeAction(BaseAction action) {
        actionPoints -= action.NeedCostActionPoints();
        OnAnyUnitActionPointsChangedAction?.Invoke();
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction action) {
        return actionPoints >= action.NeedCostActionPoints();
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action) {
        if (!CanSpendActionPointsToTakeAction(action)) {
            return false;
        }

        SpendActionPointsToTakeAction(action);
        return true;
    }

    #endregion
}