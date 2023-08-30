using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour {
    [SerializeField] private Transform crateDestructedPrefab;
    public static event Action<GridPosition> OnAnyCrateDestructedAction;
    public static void ResetStaticData() {
        OnAnyCrateDestructedAction = null;
    }

    public void Damage(int damageAmount, Vector3 damageCenter, float damageRadius) {
        var gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        OnAnyCrateDestructedAction?.Invoke(gridPosition);

        var crateDestructedTransform = Instantiate(crateDestructedPrefab, transform.position, transform.rotation);
        AddExplosionForce(crateDestructedTransform, damageAmount, damageCenter, damageRadius);

        Destroy(gameObject);
    }

    private void AddExplosionForce(Transform root, int damageAmount, Vector3 damageVector, float damageRadius) {
        var explosionForceMultiple = 10f;
        foreach (Transform child in root) {
            if (child.TryGetComponent(out Rigidbody childRigidbody)) {
                childRigidbody.AddExplosionForce(damageAmount * explosionForceMultiple, damageVector, damageRadius);
            }

            AddExplosionForce(child, damageAmount, damageVector, damageRadius);
        }
    }
}