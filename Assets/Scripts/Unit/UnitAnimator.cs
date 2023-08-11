using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    
    [SerializeField] private Unit unit;
    private Animator animator;

    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
    private static readonly int SHOOT = Animator.StringToHash("Shoot");

    private void Awake() {
        animator = GetComponent<Animator>();

        if (unit.TryGetComponent(out MoveAction moveAction)) {
            moveAction.OnMovingStartAction += MoveAction_OnMovingStartAction;
            moveAction.OnMovingStopAction += MoveAction_OnMovingStopAction;
        }

        if (unit.TryGetComponent(out ShootAction shootAction)) {
            shootAction.OnShootAction += ShootAction_OnShootAction;
        }
    }


    private void MoveAction_OnMovingStartAction() {
        animator.SetBool(IS_MOVING, true);
    }

    private void MoveAction_OnMovingStopAction() {
        animator.SetBool(IS_MOVING, false);
    }
    
    private void ShootAction_OnShootAction() {
        animator.SetTrigger(SHOOT);
    }
}