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


    private Vector3 targetPosition;

    protected override void Awake() {
        base.Awake();
        targetPosition = selfTransform.position;
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (!actionActive) {
            return;
        }

        if (Vector3.Distance(targetPosition, selfTransform.position) > 0.1f) {
            var moveDir = (targetPosition - selfTransform.position).normalized;
            selfTransform.position += moveDir * Time.deltaTime * moveSpeed;

            selfTransform.forward = Vector3.Slerp(selfTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
        } else {
            ActionComplete();
            OnMovingStopAction?.Invoke();
        }
    }

    public override string GetActionName() {
        return "Move";
    }

    public override void TakeAction(GridPosition gridPosition, Action actionCompletedCallback) {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override List<EnemyAIAction> GetEnemyAIAction() {
        var shootAction = unit.GetAction<ShootAction>();

        var enemyAiActionList = new List<EnemyAIAction>();
        var validActionGridPositions = GetValidActionGridPositions();

        const int basePriority = (int) EnemyAIActionBasePriority.Move;
        foreach (var validGridPosition in validActionGridPositions) {
            var moveGridVector = validGridPosition - unit.GetGridPosition();
            var moveGridDistance = Mathf.Abs(moveGridVector.x) + Mathf.Abs(moveGridVector.z);

            var canShootGridPositionList = shootAction.GetValidActionGridPositions(validGridPosition);
            enemyAiActionList.Add(new EnemyAIAction {
                gridPosition = validGridPosition,
                // 基础行为优先级 + 优先选择可攻击目标数量较多 + 需要移动距离最短的grid
                actionPriority = basePriority + canShootGridPositionList.Count * 100 - moveGridDistance
            });
        }

        return enemyAiActionList;
    }
}