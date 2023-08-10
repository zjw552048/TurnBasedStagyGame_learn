using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;

    private Vector3 targetPosition;

    private Transform unitTransform;

    private bool moving;

    private GridPosition gridPosition;

    private void Awake() {
        unitTransform = transform;
        targetPosition = unitTransform.position;
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);
    }

    private void Update() {
        HandleMovement();
        HandleGridRefresh();
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

    private void HandleGridRefresh() {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition == gridPosition) {
            return;
        }

        LevelGrid.Instance.MoveUnitGridPosition(this, gridPosition, newGridPosition);
        gridPosition = newGridPosition;
    }

    public void Move(Vector3 targetPos) {
        targetPosition = targetPos;
    }

    public bool IsMoving() {
        return moving;
    }
}