using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera actionVirtualCamera;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        HideActionCamera();
    }

    public void ShowActionCamera(Unit unit, Unit shootTargetUnit) {
        var unitPos = unit.GetWorldPosition();
        var shootTargetUnitPos = shootTargetUnit.GetWorldPosition();
        var shootDir = (shootTargetUnitPos - unitPos).normalized;
        var heightOffset = 1.7f;
        var shoulderOffset = 0.5f;
        var backOffset = 1f;
        /*
         * 1. unitPos, 角色位置，y为0
         * 2. Vector3.up * heightOffset, 考虑角色高度，向上偏移heightOffset
         * 3. Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffset, 考虑射击方向，绕y周正方向旋转90（射击方向的正右方），便宜shoulderOffset
         * 4. -shootDir * backOffset, 角色射击反方向，便宜backOffset
         */
        var cameraPos = unitPos +
                        Vector3.up * heightOffset +
                        Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffset +
                        -shootDir * backOffset;
        actionVirtualCamera.transform.position = cameraPos;
        actionVirtualCamera.transform.LookAt(shootTargetUnitPos + Vector3.up * heightOffset);

        actionVirtualCamera.gameObject.SetActive(true);
    }

    public void HideActionCamera() {
        actionVirtualCamera.gameObject.SetActive(false);
    }
}