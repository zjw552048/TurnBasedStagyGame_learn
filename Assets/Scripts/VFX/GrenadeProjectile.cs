using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 30;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform grenadeExplosionVfxPrefab;

    private Vector3 targetWorldPosition;
    private Action explosionCallback;

    public static event Action OnAnyGrenadeExplodedAction;

    public void SetUp(GridPosition targetPosition, Action explosionCallback) {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        this.explosionCallback = explosionCallback;
    }

    private void Update() {
        var moveDir = (targetWorldPosition - transform.position).normalized;
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        // TODO, 移动计算方式待修改

        if (Vector3.Distance(transform.position, targetWorldPosition) < 0.2f) {
            // 造成伤害
            var colliderResults = Physics.OverlapSphere(targetWorldPosition, damageRadius);
            foreach (var result in colliderResults) {
                if (result.TryGetComponent(out Unit unit)) {
                    unit.GetHealthComponent().GrenadeDamage(damageAmount, targetWorldPosition, damageRadius);
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
}