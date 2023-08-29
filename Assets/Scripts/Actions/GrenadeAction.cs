using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction {
    [SerializeField] private int maxThrowGrid = 9;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform grenadeProjectilePrefab;

    private Vector3 targetWorldPosition;

    public override string GetActionName() {
        return "Grenade";
    }

    private void Update() {
        if (!actionActive) {
            return;
        }

        var dir = (targetWorldPosition - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * rotateSpeed);
        
        Debug.Log(dir);
        Debug.Log(transform.forward);
        Debug.Log(transform.eulerAngles);
    }

    public override void TakeAction(GridPosition targetGridPosition, Action actionCompletedCallback) {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        var grenadeProjectileTransform =
            Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        var grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.SetUp(targetGridPosition, GrenadeExplosionCallback);

        ActionStart(actionCompletedCallback);
    }

    private void GrenadeExplosionCallback() {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        var baseGridPosition = unit.GetGridPosition();
        var validGridPositionList = new List<GridPosition>();
        for (var x = -maxThrowGrid; x <= maxThrowGrid; x++) {
            for (var z = -maxThrowGrid; z <= maxThrowGrid; z++) {
                var testGridPosition = baseGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxThrowGrid) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override List<EnemyAIAction> GetEnemyAIAction() {
        // TODO
        return new List<EnemyAIAction> {
            new() {
                gridPosition = unit.GetGridPosition(),
                actionPriority = 0
            }
        };
    }
}