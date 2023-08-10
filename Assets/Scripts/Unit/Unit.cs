using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed= 10f;

    private Vector3 targetPosition;

    private Transform unitTransform;

    private bool moving;

    private void Awake() {
        unitTransform = transform;
        targetPosition = unitTransform.position;
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (Vector3.Distance(targetPosition, unitTransform.position) > 0.1f) {
            var moveDir = (targetPosition - unitTransform.position).normalized;
            unitTransform.position += moveDir * Time.deltaTime * moveSpeed;
            
            unitTransform.forward = Vector3.Slerp(unitTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
            moving = true;
        } else {
            moving = false;
        }
    }

    public void Move(Vector3 targetPos) {
        targetPosition = targetPos;
    }

    public bool IsMoving() {
        return moving;
    }
}