using UnityEngine;

public class Unit : MonoBehaviour {
    private const float MOVE_SPEED = 10f;

    private Vector3 targetPosition;

    private Transform unitTransform;

    private void Awake() {
        unitTransform = transform;
    }

    private void Update() {
        if (Vector3.Distance(targetPosition, unitTransform.position) > 0.1f) {
            var moveDir = (targetPosition - unitTransform.position).normalized;
            unitTransform.position += moveDir * Time.deltaTime * MOVE_SPEED;
        }

        if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPos) {
        targetPosition = targetPos;
    }
}