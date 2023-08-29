using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private bool isOpen;
    [SerializeField] private float interactDuration = 0.5f;

    private GridPosition gridPosition;
    private bool isInteracting;
    private float interactTimer;
    private Action interactCallback;

    private Animator animator;
    private static readonly int IS_OPEN = Animator.StringToHash("IsOpen");

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(this, gridPosition);

        if (isOpen) {
            DoorOpen();
        } else {
            DoorClose();
        }
    }

    private void Update() {
        if (!isInteracting) {
            return;
        }

        interactTimer -= Time.deltaTime;
        if (interactTimer > 0) {
            return;
        }

        isInteracting = false;
        interactCallback?.Invoke();
    }

    public void Interact(Action interactCallback) {
        this.interactCallback = interactCallback;
        isInteracting = true;
        interactTimer = interactDuration;

        isOpen = !isOpen;
        if (isOpen) {
            DoorOpen();
        } else {
            DoorClose();
        }

        Debug.Log("door interact, isOpen:" + isOpen);
    }

    private void DoorOpen() {
        animator.SetBool(IS_OPEN, isOpen);
        PathfindingManager.Instance.SetWalkable(gridPosition, isOpen);
    }

    private void DoorClose() {
        animator.SetBool(IS_OPEN, isOpen);
        PathfindingManager.Instance.SetWalkable(gridPosition, isOpen);
    }
}