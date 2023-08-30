using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Unit unit;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private Animator animator;

    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
    private static readonly int SHOOT = Animator.StringToHash("Shoot");
    private static readonly int SWORD_SLASH = Animator.StringToHash("SwordSlash");

    private void Awake() {
        animator = GetComponent<Animator>();

        if (unit != null) {
            if (unit.TryGetComponent(out MoveAction moveAction)) {
                moveAction.OnMovingStartAction += MoveAction_OnMovingStartAction;
                moveAction.OnMovingStopAction += MoveAction_OnMovingStopAction;
            }

            if (unit.TryGetComponent(out ShootAction shootAction)) {
                shootAction.OnShootAction += ShootAction_OnShootAction;
            }

            if (unit.TryGetComponent(out SwordAction swordAction)) {
                swordAction.OnSwingStartAction += SwordAction_OnSwingStartAction;
                swordAction.OnSwingStopAction += SwordAction_OnSwingStopAction;
            }
        }
    }

    private void Start() {
        EquipRifle();
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

    private void SwordAction_OnSwingStartAction() {
        animator.SetTrigger(SWORD_SLASH);
        EquipSword();
    }

    private void SwordAction_OnSwingStopAction() {
        EquipRifle();
    }

    private void EquipRifle() {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }

    private void EquipSword() {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }
}