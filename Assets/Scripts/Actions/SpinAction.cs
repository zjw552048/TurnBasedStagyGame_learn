using System;
using UnityEngine;

public class SpinAction : BaseAction {
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
            onActionCompletedAction?.Invoke();
        }

        transform.eulerAngles += new Vector3(0, rotateAngle, 0);
    }

    public void Spin(Action actionCompletedCallback) {
        actionActive = true;
        targetSpinAngle = 360f;
        onActionCompletedAction = actionCompletedCallback;
    }
    
    public override string GetActionName() {
        return "Spin";
    }
}