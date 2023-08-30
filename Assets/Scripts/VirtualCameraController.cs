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
        var inputVector = InputManager.Instance.GetMovementInputVector();
        var moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation() {
        var rotateVector = new Vector3();
        rotateVector.y = InputManager.Instance.GetRotationInput();
        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }

    private void HandleZoom() {
        targetCameraFollowOffsetY += InputManager.Instance.GetZoomInput();
        targetCameraFollowOffsetY = Mathf.Clamp(targetCameraFollowOffsetY, minFollowOffsetY, maxFollowOffsetY);

        var followOffset = transposer.m_FollowOffset;
        followOffset.y = Mathf.SmoothStep(followOffset.y, targetCameraFollowOffsetY, zoomSpeed * Time.deltaTime);
        transposer.m_FollowOffset = followOffset;
    }
}