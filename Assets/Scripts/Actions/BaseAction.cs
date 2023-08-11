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

    public bool IsValidMoveActionGridPosition(GridPosition gridPosition) {
        var validGridPositionList = GetValidMoveActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidMoveActionGridPositions();

    public virtual int NeedCostActionPoints() {
        return 1;
    }
}