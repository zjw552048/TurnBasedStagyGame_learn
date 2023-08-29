using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class InteractAction : BaseAction {
    [SerializeField] private int maxInteractGrid = 1;

    public override string GetActionName() {
        return "Interact";
    }

    private void Update() {
        if (!actionActive) {
            return;
        }
        
        ActionComplete();
    }

    public override void TakeAction(GridPosition targetGridPosition, Action actionCompletedCallback) {
        ActionStart(actionCompletedCallback);
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        var baseGridPosition = unit.GetGridPosition();
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxInteractGrid; x <= maxInteractGrid; x++) {
            for (var z = -maxInteractGrid; z <= maxInteractGrid; z++) {
                var testGridPosition = baseGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxInteractGrid) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        const int basePriority = (int) EnemyAIActionBasePriority.Interact;
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionPriority = basePriority
        };
    }
}