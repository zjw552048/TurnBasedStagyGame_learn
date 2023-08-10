using UnityEngine;

public class UnitVisual : MonoBehaviour {
    [SerializeField] private Unit unit;
    private Animator animator;

    private bool unitIsMoving;
    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (unitIsMoving == unit.IsMoving()) {
            return;
        }

        unitIsMoving = !unitIsMoving;
        animator.SetBool(IS_MOVING, unitIsMoving);
    }
}