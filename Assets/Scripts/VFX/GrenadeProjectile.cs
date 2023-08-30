using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 30;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform grenadeExplosionVfxPrefab;

    [SerializeField] private AnimationCurve shortRangePosYCurve;
    [SerializeField] private AnimationCurve longRangePosYCurve;
    [SerializeField] private AnimationCurve superLongRangePosYCurve;

    private Vector3 targetWorldPosition;
    private Action explosionCallback;

    private float xzMoveDistance;
    private float xzMoveDistanceLeft;
    private float maxY;
    private Vector3 xzMoveDir;
    private Vector3 xzPosition;

    public static event Action OnAnyGrenadeExplodedAction;
    public static void ResetStaticData() {
        OnAnyGrenadeExplodedAction = null;
    }


    public void SetUp(GridPosition targetPosition, Action explosionCallback) {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        this.explosionCallback = explosionCallback;

        var xyBornPos = transform.position;
        xyBornPos.y = 0;

        xzMoveDistance = Vector3.Distance(targetWorldPosition, xyBornPos);
        xzMoveDistanceLeft = xzMoveDistance;
        maxY = xzMoveDistance / 3f;

        xzMoveDir = (targetWorldPosition - xyBornPos).normalized;
        xzPosition = xyBornPos;
    }

    private void Update() {
        var distance = moveSpeed * Time.deltaTime;
        if (xzMoveDistanceLeft > distance) {
            xzMoveDistanceLeft -= distance;
            xzPosition += xzMoveDir * distance;

            #region 确保初始高度为Unit.HEIGHT_OFFSET,同时通过curve的比例也固定了最大高度

            var posYCurve = xzMoveDistance switch {
                < 8 => shortRangePosYCurve,
                < 14 => longRangePosYCurve,
                _ => superLongRangePosYCurve
            };

            var timeNormalized = 1 - xzMoveDistanceLeft / xzMoveDistance;
            var yNormalized = posYCurve.Evaluate(timeNormalized);
            var yNormalizedZero = posYCurve.Evaluate(0);
            var y = Unit.HEIGHT_OFFSET / yNormalizedZero * yNormalized;

            #endregion

            transform.position = new Vector3(xzPosition.x, y, xzPosition.z);

            return;
        }

        transform.position = targetWorldPosition;

        // 造成伤害
        var colliderResults = Physics.OverlapSphere(targetWorldPosition, damageRadius);
        foreach (var result in colliderResults) {
            if (result.TryGetComponent(out Unit unit)) {
                unit.GetHealthComponent().GrenadeDamage(damageAmount, targetWorldPosition, damageRadius);
            }

            if (result.TryGetComponent(out DestructableCrate crate)) {
                crate.Damage(damageAmount, targetWorldPosition, damageRadius);
            }
        }

        explosionCallback?.Invoke();

        OnAnyGrenadeExplodedAction?.Invoke();

        Instantiate(grenadeExplosionVfxPrefab, targetWorldPosition + Vector3.up, Quaternion.identity);

        // 避免拖尾突然销毁:取消父节点，勾选Autodestruct自动销毁
        trailRenderer.transform.parent = null;

        Destroy(gameObject);
    }
}