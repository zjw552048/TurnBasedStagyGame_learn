using UnityEngine;

public class VirtualCameraController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 100f;

    private void Update() {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
        var inputVector = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            inputVector.z = 1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            inputVector.z = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = 1f;
        }

        var moveDir = transform.forward * inputVector.z + transform.right * inputVector.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation() {
        var rotateVector = new Vector3();
        if (Input.GetKey(KeyCode.Q)) {
            rotateVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.E)) {
            rotateVector.y = -1f;
        }

        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }
}