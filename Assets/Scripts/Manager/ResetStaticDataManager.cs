using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {
    private void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
        SwordAction.ResetStaticData();
        Unit.ResetStaticData();
        BulletProjectile.ResetStaticData();
        GrenadeProjectile.ResetStaticData();
        DestructableCrate.ResetStaticData();
    }
}