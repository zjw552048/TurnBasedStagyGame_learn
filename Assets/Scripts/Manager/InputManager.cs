using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Vector3 GetMousePosition() {
        return Input.mousePosition;
    }

    public bool IsMouseLeftButtonDown() {
        return Input.GetMouseButtonDown(0);
    }

    public Vector3 GetMovementInputVector() {
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

        return inputVector;
    }

    public float GetRotationInputVector() {
        var rotateAmount = 0f;
        if (Input.GetKey(KeyCode.Q)) {
            rotateAmount = 1f;
        }

        if (Input.GetKey(KeyCode.E)) {
            rotateAmount = -1f;
        }

        return rotateAmount;
    }

    public float GetZoomInput() {
        if (Input.mouseScrollDelta.y > 0) {
            return 1;
        }

        if (Input.mouseScrollDelta.y < 0) {
            return -1;
        }

        return 0;
    }
}