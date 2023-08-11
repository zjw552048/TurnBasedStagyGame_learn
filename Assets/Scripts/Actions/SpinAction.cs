using System;
using System.Collections.Generic;
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

    public override void TakeAction(GridPosition gridPosition, Action actionCompletedCallback) {
        targetSpinAngle = 360f;
        
        ActionStart(actionCompletedCallback);
    }

    public override List<GridPosition> GetValidMoveActionGridPositions() {
        return new List<GridPosition> {
            unit.GetGridPosition()
        };
    }

    public override int NeedCostActionPoints() {
        return 2;
    }
}