#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    private NewInputActions newInputActions;

    private void Awake() {
        Instance = this;

        newInputActions = new NewInputActions();
        newInputActions.Player.Enable();
    }

    public Vector3 GetMousePosition() {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseLeftButtonDownThisFrame() {
#if USE_NEW_INPUT_SYSTEM
        return newInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetMovementInputVector() {
#if USE_NEW_INPUT_SYSTEM
        return newInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        var inputVector = new Vector2();
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = 1f;
        }

        return inputVector;
#endif
    }

    public float GetRotationInputVector() {
#if USE_NEW_INPUT_SYSTEM
        return newInputActions.Player.CameraRotation.ReadValue<float>();
#else
        var rotateAmount = 0f;
        if (Input.GetKey(KeyCode.Q)) {
            rotateAmount = 1f;
        }

        if (Input.GetKey(KeyCode.E)) {
            rotateAmount = -1f;
        }

        return rotateAmount;
#endif
    }

    public float GetZoomInput() {
#if USE_NEW_INPUT_SYSTEM
        return newInputActions.Player.CameraZoom.ReadValue<float>();
#else
        if (Input.mouseScrollDelta.y > 0) {
            return 1;
        }

        if (Input.mouseScrollDelta.y < 0) {
            return -1;
        }

        return 0;
#endif
    }
}