using UnityEngine;

public class SpinAction : BaseAction{
    [SerializeField] private float spinSpeed = 360f;

    private float targetSpinAngle;

    private void Update() {
        if (!actionActive) {
            return;
        }

        var rotateAngle = spinSpeed * Time.deltaTime;
        if (targetSpinAngle > rotateAngle) {
            targetSpinAngle -= rotateAngle;
        } else {
            rotateAngle = targetSpinAngle;
            targetSpinAngle = 0f;
            actionActive = false;
        }

        transform.eulerAngles += new Vector3(0, rotateAngle, 0);
    }

    public void Spin() {
        actionActive = true;
        targetSpinAngle = 360f;
    }
}