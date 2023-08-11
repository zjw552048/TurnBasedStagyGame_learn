using UnityEngine;

public class Unit : MonoBehaviour {
    private GridPosition gridPosition;

    private BaseAction[] baseActionArray;

    private int actionPoints;

    private void Awake() {
        baseActionArray = GetComponents<BaseAction>();

        actionPoints = 2;
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);
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

    public int GetActionPoints() {
        return actionPoints;
    }

    private void SpendActionPointsToTakeAction(BaseAction action) {
        actionPoints -= action.NeedCostActionPoints();
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