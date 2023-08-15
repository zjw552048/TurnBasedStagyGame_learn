using UnityEngine;

public class ScreenShakeListener : MonoBehaviour {
    private void Start() {
        ShootAction.OnAnyShootAction += ShootAction_OnAnyShootAction;
    }

    private void ShootAction_OnAnyShootAction() {
        ScreenShake.Instance.ScreenShakeWithForce();
    }
}