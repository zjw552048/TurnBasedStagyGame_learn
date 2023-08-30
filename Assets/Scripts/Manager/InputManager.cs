#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    private NewInputActions newInputActions;

    private bool isMobilePlatform;

    private void Awake() {
        Instance = this;

        newInputActions = new NewInputActions();
        newInputActions.Player.Enable();

        isMobilePlatform = Application.isMobilePlatform;
    }

    public Vector3 GetMousePosition() {
#if USE_NEW_INPUT_SYSTEM
        if (isMobilePlatform) {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        } else {
            return Mouse.current.position.ReadValue();
        }
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseLeftButtonDownThisFrame() {
#if USE_NEW_INPUT_SYSTEM
        if (isMobilePlatform) {
            return Touchscreen.current.primaryTouch.press.isPressed;
        } else {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }

#else
        if (isMobilePlatform) {
            return Input.GetTouch(0).phase == TouchPhase.Began;
        } else {
            return Input.GetMouseButtonDown(0);
        }
#endif
    }

    public bool IsPointerOverGameObject() {
#if USE_NEW_INPUT_SYSTEM
        if (isMobilePlatform) {
            return EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue());
        } else {
            return EventSystem.current.IsPointerOverGameObject();
        }
#else
        if (isMobilePlatform) {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        } else {
            return EventSystem.current.IsPointerOverGameObject();
        }
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

    public float GetRotationInput() {
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

    public bool IsAdjustCamera() {
        return GetMovementInputVector() != Vector2.zero ||
               GetRotationInput() != 0 ||
               GetZoomInput() != 0;
    }
}