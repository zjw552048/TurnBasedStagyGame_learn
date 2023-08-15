using UnityEngine;

public class ScreenShakeListener : MonoBehaviour {
    private void Start() {
        BulletProjectile.OnAnyBulletFiredAction += BulletProjectile_OnAnyBulletFriedAction;
        GrenadeProjectile.OnAnyGrenadeExplodedAction += GrenadeProjectile_OnAnyGrenadeExplodedAction;
    }

    private void BulletProjectile_OnAnyBulletFriedAction() {
        ScreenShake.Instance.ScreenShakeWithForce();
    }

    private void GrenadeProjectile_OnAnyGrenadeExplodedAction() {
        ScreenShake.Instance.ScreenShakeWithForce(5);
    }
}