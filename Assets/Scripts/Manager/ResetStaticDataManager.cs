using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {
    private void Start() {
        SwordAction.ResetStaticData();
        Unit.ResetStaticData();
        BulletProjectile.ResetStaticData();
        GrenadeProjectile.ResetStaticData();
        DestructableCrate.ResetStaticData();
    }
}