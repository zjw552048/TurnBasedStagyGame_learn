using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damageAmount = 30;

    private Vector3 targetWorldPosition;

    public void SetUp(GridPosition targetPosition) {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
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

            Destroy(gameObject);
        }
    }
}