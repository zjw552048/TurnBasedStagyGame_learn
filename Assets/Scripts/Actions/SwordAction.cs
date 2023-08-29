using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction {
    [SerializeField] private int maxSwordSlashGrid = 1;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int damageAmount = 100;

    public static event Action OnAnySwordHitAction;
    public event Action OnSwingStartAction;
    public event Action OnSwingStopAction;

    private enum State {
        SwingSwordBeforeHit,
        SwingSwordAfterHit,
    }

    private State state;
    private float actionTimer;
    private const float SWING_SWORD_BEFORE_HIT_TIME = 0.5f;
    private const float SWING_SWORD_AFTER_HIT_TIME = 0.5f;

    private Unit swordTargetUnit;

    private void Update() {
        if (!actionActive) {
            return;
        }

        switch (state) {
            case State.SwingSwordBeforeHit:
                var dir = (swordTargetUnit.transform.position - transform.position).normalized;
                transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * rotateSpeed);
                break;

            case State.SwingSwordAfterHit:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        actionTimer -= Time.deltaTime;
        if (actionTimer > 0) {
            return;
        }

        NextStateLogic();
    }

    private void NextStateLogic() {
        switch (state) {
            case State.SwingSwordBeforeHit:
                actionTimer = SWING_SWORD_AFTER_HIT_TIME;
                state = State.SwingSwordAfterHit;

                var dir = (swordTargetUnit.transform.position - transform.position).normalized;
                swordTargetUnit.GetHealthComponent().SwordDamage(damageAmount, dir);

                OnAnySwordHitAction?.Invoke();
                break;

            case State.SwingSwordAfterHit:
                OnSwingStopAction?.Invoke();
                ActionComplete();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string GetActionName() {
        return "Sword";
    }

    public override void TakeAction(GridPosition targetGridPosition, Action actionCompletedCallback) {
        swordTargetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);

        actionTimer = SWING_SWORD_BEFORE_HIT_TIME;
        state = State.SwingSwordBeforeHit;

        OnSwingStartAction?.Invoke();

        ActionStart(actionCompletedCallback);
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        var baseGridPosition = unit.GetGridPosition();
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxSwordSlashGrid; x <= maxSwordSlashGrid; x++) {
            for (var z = -maxSwordSlashGrid; z <= maxSwordSlashGrid; z++) {
                var testGridPosition = baseGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxSwordSlashGrid) {
                    continue;
                }

                var unitAtGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unitAtGridPosition == null) {
                    continue;
                }

                if (unitAtGridPosition.IsPlayer() == unit.IsPlayer()) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public List<GridPosition> GetValidActionRangeGridPositions() {
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxSwordSlashGrid; x <= maxSwordSlashGrid; x++) {
            for (var z = -maxSwordSlashGrid; z <= maxSwordSlashGrid; z++) {
                var unitGridPosition = unit.GetGridPosition();
                var testGridPosition = unitGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxSwordSlashGrid) {
                    continue;
                }

                var unitAtTestGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (unitAtTestGridPosition != null && unitAtTestGridPosition.IsPlayer() == unit.IsPlayer()) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override List<EnemyAIAction> GetEnemyAIAction() {
        var enemyAiActionList = new List<EnemyAIAction>();
        var validActionGridPosition = GetValidActionGridPositions();

        foreach (var gridPosition in validActionGridPosition) {
            enemyAiActionList.Add(new EnemyAIAction {
                    gridPosition = gridPosition,
                    actionPriority = 200
                }
            );
        }

        return enemyAiActionList;
    }
}