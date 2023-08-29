using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

public class MoveAction : BaseAction {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int maxMoveGrid = 3;

    public event Action OnMovingStartAction;
    public event Action OnMovingStopAction;


    private List<Vector3> pathPositionList;
    private int currentPositionIndex;

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (!actionActive) {
            return;
        }

        var targetPosition = pathPositionList[currentPositionIndex];
        if (Vector3.Distance(targetPosition, selfTransform.position) > 0.1f) {
            var moveDir = (targetPosition - selfTransform.position).normalized;
            selfTransform.position += moveDir * Time.deltaTime * moveSpeed;

            selfTransform.forward = Vector3.Slerp(selfTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
        } else {
            currentPositionIndex++;
            if (currentPositionIndex < pathPositionList.Count) {
                return;
            }

            ActionComplete();
            OnMovingStopAction?.Invoke();
        }
    }

    public override string GetActionName() {
        return "Move";
    }

    public override void TakeAction(GridPosition targetGridPosition, Action actionCompletedCallback) {
        var gridPositionList =
            PathfindingManager.Instance.FindPath(unit.GetGridPosition(), targetGridPosition, out var pathDistance);
        pathPositionList = new List<Vector3>();
        foreach (var gridPosition in gridPositionList) {
            pathPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        currentPositionIndex = 0;

        ActionStart(actionCompletedCallback);
        OnMovingStartAction?.Invoke();
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        var unitGridPosition = unit.GetGridPosition();
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxMoveGrid; x <= maxMoveGrid; x++) {
            for (var z = -maxMoveGrid; z <= maxMoveGrid; z++) {
                var testGridPosition = unitGridPosition + new GridPosition(x, z);

                if (testGridPosition == unitGridPosition) {
                    continue;
                }

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                var unitAtTestGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unitAtTestGridPosition != null) {
                    continue;
                }

                if (!PathfindingManager.Instance.IsWalkable(testGridPosition)) {
                    continue;
                }

                var pathDistance = PathfindingManager.Instance.GetPathDistance(unitGridPosition, testGridPosition);
                var moveCostMultiple = PathfindingManager.MOVE_COST_MULTIPLE;
                if (pathDistance < 0 || pathDistance > maxMoveGrid * moveCostMultiple) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        var shootAction = unit.GetAction<ShootAction>();


        const int basePriority = (int) EnemyAIActionBasePriority.Move;
        var moveGridVector = gridPosition - unit.GetGridPosition();
        var moveGridDistance = Mathf.Abs(moveGridVector.x) + Mathf.Abs(moveGridVector.z);

        var canShootGridPositionList = shootAction.GetValidActionGridPositions(gridPosition);
        return new EnemyAIAction {
            gridPosition = gridPosition,
            // 基础行为优先级 + 优先选择可攻击目标数量较多 + 需要移动距离最短的grid
            actionPriority = basePriority + canShootGridPositionList.Count * 100 - moveGridDistance
        };
    }
}