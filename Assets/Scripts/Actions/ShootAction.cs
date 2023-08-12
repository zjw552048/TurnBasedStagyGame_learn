using System;
using System.Collections.Generic;
using Enum;
using Unity.Mathematics;
using UnityEngine;

public class ShootAction : BaseAction {
    [SerializeField] private int maxShootGrid = 7;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform gunFirePoint;
    [SerializeField] private Transform bulletProjectilePrefab;

    public event Action OnShootAction;

    private enum State {
        Aiming,
        Shooting,
        CoolOff,
    }

    private State state;

    private Unit shootTargetUnit;
    private bool canShootBullet;

    private float actionTimer;
    private const float AIMING_TIMER = 0.5f;
    private const float SHOOTING_TIMER = 0.1f;
    private const float COOL_OFF_TIMER = 1f;

    private void Update() {
        if (!actionActive) {
            return;
        }

        actionTimer -= Time.deltaTime;

        switch (state) {
            case State.Aiming:
                var dir = (shootTargetUnit.GetWorldPosition() - transform.position).normalized;
                transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * rotateSpeed);
                break;

            case State.Shooting:
                if (canShootBullet) {
                    canShootBullet = false;
                    Shoot();
                    OnShootAction?.Invoke();
                }

                break;

            case State.CoolOff:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (actionTimer > 0) {
            return;
        }

        NextStateLogic();
    }

    private void NextStateLogic() {
        switch (state) {
            case State.Aiming:
                state = State.Shooting;
                actionTimer = SHOOTING_TIMER;
                break;

            case State.Shooting:
                state = State.CoolOff;
                actionTimer = COOL_OFF_TIMER;
                break;

            case State.CoolOff:
                ActionComplete();
                CameraManager.Instance.HideActionCamera();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Shoot() {
        var bulletProjectTransform = Instantiate(bulletProjectilePrefab, gunFirePoint.position, quaternion.identity);
        var bulletProjectile = bulletProjectTransform.GetComponent<BulletProjectile>();
        bulletProjectile.SetDamageTarget(shootTargetUnit.GetHealthComponent());
    }

    public override string GetActionName() {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action actionCompletedCallback) {
        state = State.Aiming;
        actionTimer = AIMING_TIMER;

        shootTargetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;

        ActionStart(actionCompletedCallback);
        CameraManager.Instance.ShowActionCamera(unit, shootTargetUnit);
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        var unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositions(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition gridPosition) {
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxShootGrid; x <= maxShootGrid; x++) {
            for (var z = -maxShootGrid; z <= maxShootGrid; z++) {
                var testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxShootGrid) {
                    continue;
                }

                var unitAtTestGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unitAtTestGridPosition == null) {
                    continue;
                }

                if (unitAtTestGridPosition.IsPlayer() == unit.IsPlayer()) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public List<GridPosition> GetValidActionRangeGridPositions() {
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxShootGrid; x <= maxShootGrid; x++) {
            for (var z = -maxShootGrid; z <= maxShootGrid; z++) {
                var unitGridPosition = unit.GetGridPosition();
                var testGridPosition = unitGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxShootGrid) {
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
        var enemyAiActionList = new List<EnemyAIAction>();
        var validActionGridPositions = GetValidActionGridPositions();

        const int basePriority = (int) EnemyAIActionBasePriority.Shoot;
        foreach (var validGridPosition in validActionGridPositions) {
            var unitAtGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition);
            var healthNormalized = unitAtGridPosition.GetHealthComponent().GetHealthNormalized();
            enemyAiActionList.Add(new EnemyAIAction {
                gridPosition = validGridPosition,
                // 基础行为优先级 + 优先选择剩余血量较低目标
                actionPriority = basePriority + (int) ((1 - healthNormalized) * 100)
            });
        }

        return enemyAiActionList;
    }
}