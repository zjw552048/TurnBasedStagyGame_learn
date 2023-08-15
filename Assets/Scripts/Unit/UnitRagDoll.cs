using System;
using Enum;
using UnityEngine;

public class UnitRagDoll : MonoBehaviour {
    [SerializeField] private Transform boneRoot;
    [SerializeField] private Rigidbody boneSpine;

    public void SetUp(Transform originRoot, HealthComponent.OnHealthZeroActionArgs args) {
        // 同步骨骼Transform
        SyncBoneTransform(originRoot, boneRoot);

        switch (args.damageType) {
            case DamageType.ShootDamage:
                ApplyForce(args.damageAmount, args.damageVector);
                break;

            case DamageType.GrenadeDamage:
                AddExplosionForce(boneRoot, args.damageAmount, args.damageVector, args.damageRadius);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(args.damageType), args.damageType, null);
        }
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

    private void ApplyForce(int damageAmount, Vector3 damageVector) {
        boneSpine.AddForce(damageAmount * damageVector, ForceMode.Impulse);
    }

    private void AddExplosionForce(Transform root, int damageAmount, Vector3 damageVector, float damageRadius) {
        var explosionForceMultiple = 30f;
        foreach (Transform child in root) {
            if (child.TryGetComponent(out Rigidbody childRigidbody)) {
                childRigidbody.AddExplosionForce(damageAmount * explosionForceMultiple, damageVector, damageRadius);
            }

            AddExplosionForce(child, damageAmount, damageVector, damageRadius);
        }
    }
}