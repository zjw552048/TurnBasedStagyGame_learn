using UnityEngine;

public class BulletProjectile : MonoBehaviour {
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;

    private float moveDistance;
    private Vector3 moveDir;

    public void SetTargetPos(Vector3 targetPos) {
        var bornPos = transform.position;
        targetPos.y = bornPos.y;

        moveDistance = Vector3.Distance(targetPos, bornPos);
        moveDir = (targetPos - bornPos).normalized;
    }

    private void Update() {
        var distance = moveSpeed * Time.deltaTime;
        if (moveDistance > distance) {
            moveDistance -= distance;
            transform.position += distance * moveDir;
            return;
        }

        // 到达目标点
        transform.position += moveDistance * moveDir;
        // 拖尾组件分离，等待自动销毁
        trailRenderer.transform.parent = null;
        // 销毁弹头
        Destroy(gameObject);
        // 实例化击中特效
        Instantiate(bulletHitVFXPrefab, transform.position, Quaternion.identity);
    }
}