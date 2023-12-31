using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class SpinAction : BaseAction {
    [SerializeField] private float spinSpeed = 360f;

    private float targetSpinAngle;

    private void Update() {
        if (!actionActive) {
            return;
        }

        var rotateAngle = spinSpeed * Time.deltaTime;
        if (targetSpinAngle > rotateAngle) {
            targetSpinAngle -= rotateAngle;
        } else {
            rotateAngle = targetSpinAngle;
            targetSpinAngle = 0f;

            ActionComplete();
        }

        transform.eulerAngles += new Vector3(0, rotateAngle, 0);
    }

    public override string GetActionName() {
        return "Spin";
    }

    public override void TakeAction(GridPosition targetGridPosition, Action actionCompletedCallback) {
        targetSpinAngle = 360f;

        ActionStart(actionCompletedCallback);
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        return new List<GridPosition> {
            unit.GetGridPosition()
        };
    }

    public override int NeedCostActionPoints() {
        return 2;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        const int basePriority = (int) EnemyAIActionBasePriority.Spin;
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionPriority = basePriority
        };
    }
}