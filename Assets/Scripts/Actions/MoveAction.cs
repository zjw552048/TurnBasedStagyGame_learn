using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int maxMoveGrid = 1;


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
            actionActive = false;
            onActionCompletedAction?.Invoke();
        }
    }

    public bool IsMoving() {
        return actionActive;
    }

    public void Move(GridPosition gridPosition, Action actionCompletedCallback) {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        actionActive = true;
        onActionCompletedAction = actionCompletedCallback;
    }

    public bool IsValidMoveActionGridPosition(GridPosition gridPosition) {
        var validGridPositionList = GetValidMoveActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidMoveActionGridPositions() {
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

                if (LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition)) {
                    continue;
                }

                // Debug.Log($"x:{x}, z:{z}, maxMoveGrid:{maxMoveGrid}, " + new GridPosition(x, z));
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
    
    public override string GetActionName() {
        return "Move";
    }
}