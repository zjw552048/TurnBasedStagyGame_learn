using UnityEngine;

public class UnitRagDoll : MonoBehaviour {
    [SerializeField] private Transform boneRoot;
    [SerializeField] private Rigidbody boneSpine;

    public void SetUp(Transform originRoot, Vector3 damageForce) {
        // 同步骨骼Transform
        SyncBoneTransform(originRoot, boneRoot);
        // 对目标施加Force
        ApplyForce(damageForce);
    }

    private void SyncBoneTransform(Transform originRoot, Transform cloneRoot) {
        for (var i = 0; i < originRoot.childCount; i++) {
            var originChildTransform = originRoot.GetChild(i);
            var cloneChildTransform = cloneRoot.GetChild(i);

            cloneChildTransform.position = originChildTransform.position;
            cloneChildTransform.rotation = originChildTransform.rotation;

            if (originChildTransform.childCount <= 0) {
                return;
            }

            SyncBoneTransform(originChildTransform, cloneChildTransform);
        }
    }

    private void ApplyForce(Vector3 damageForce) {
        boneSpine.AddForce(damageForce, ForceMode.Impulse);
    }
}