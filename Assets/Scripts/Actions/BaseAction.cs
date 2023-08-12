using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    protected Unit unit;
    protected Transform selfTransform;
    protected bool actionActive;

    protected Action actionCompletedCallback;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
        selfTransform = unit.transform;
    }

    public bool IsActionActive() {
        return actionActive;
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action actionCompletedCallback);

    public bool IsValidActionGridPosition(GridPosition gridPosition) {
        var validGridPositionList = GetValidActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositions();

    public virtual int NeedCostActionPoints() {
        return 1;
    }

    protected void ActionStart(Action actionCompletedCallback) {
        actionActive = true;
        this.actionCompletedCallback = actionCompletedCallback;
    }

    protected void ActionComplete() {
        actionActive = false;
        actionCompletedCallback?.Invoke();
    }

    public abstract List<EnemyAIAction> GetEnemyAIAction();

    public EnemyAIAction GetBestEnemyAIAction() {
        var validActionList = GetEnemyAIAction();
        if (validActionList.Count <= 0) {
            return default;
        }

        validActionList.Sort((a, b) => b.actionPriority - a.actionPriority);
        return validActionList[0];
    }
}