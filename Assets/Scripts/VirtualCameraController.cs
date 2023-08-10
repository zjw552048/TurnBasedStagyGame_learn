using System;
using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minFollowOffsetY = 2f;
    [SerializeField] private float maxFollowOffsetY = 12f;

    private CinemachineTransposer transposer;
    private float targetCameraFollowOffsetY;

    private void Awake() {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetCameraFollowOffsetY = transposer.m_FollowOffset.y;
    }


    private void Update() {
        HandleMovement();
        HandleRotation();
        HandleZoom();
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

    private void HandleZoom() {
        var followOffset = transposer.m_FollowOffset;
        if (Input.mouseScrollDelta.y > 0) {
            targetCameraFollowOffsetY += 1f;
        }

        if (Input.mouseScrollDelta.y < 0) {
            targetCameraFollowOffsetY -= 1f;
        }

        targetCameraFollowOffsetY = Mathf.Clamp(targetCameraFollowOffsetY, minFollowOffsetY, maxFollowOffsetY);

        followOffset.y = Mathf.SmoothStep(followOffset.y, targetCameraFollowOffsetY, zoomSpeed * Time.deltaTime);
        transposer.m_FollowOffset = followOffset;
    }
}