using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 targetPosition;

    private Transform unitTransform;

    private bool moving;
    private void Awake() {
        unitTransform = transform;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Move(MouseWorld.GetPosition());
        }

        HandleMovement();
    }

    private void HandleMovement() {
        if (Vector3.Distance(targetPosition, unitTransform.position) > 0.1f) {
            var moveDir = (targetPosition - unitTransform.position).normalized;
            unitTransform.position += moveDir * Time.deltaTime * moveSpeed;
            
            moving = true;
        } else {
            moving = false;
        }
    }

    private void Move(Vector3 targetPos) {
        targetPosition = targetPos;
    }

    public bool IsMoving() {
        return moving;
    }
}