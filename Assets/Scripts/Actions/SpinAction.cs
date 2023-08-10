using UnityEngine;

public class SpinAction : MonoBehaviour {
    [SerializeField] private float spinSpeed = 360f;

    private bool spinning;
    private float targetSpinAngle;

    private void Update() {
        if (!spinning) {
            return;
        }

        var rotateAngle = spinSpeed * Time.deltaTime;
        if (targetSpinAngle > rotateAngle) {
            targetSpinAngle -= rotateAngle;
        } else {
            rotateAngle = targetSpinAngle;
            targetSpinAngle = 0f;
            spinning = false;
        }

        transform.eulerAngles += new Vector3(0, rotateAngle, 0);
    }

    public void Spin() {
        spinning = true;
        targetSpinAngle = 360f;
    }
}