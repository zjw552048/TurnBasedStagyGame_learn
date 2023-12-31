using Enum;
using UnityEngine;

public class UnitRagDollSpawner : MonoBehaviour {
    [SerializeField] private Transform unitRagDollPrefab;
    [SerializeField] private Transform boneRoot;

    private HealthComponent healthComponent;

    private void Awake() {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void Start() {
        healthComponent.OnHealthZeroAction += HealthComponent_OnHealthZeroAction;
    }

    private void HealthComponent_OnHealthZeroAction(object sender, HealthComponent.OnHealthZeroActionArgs args) {
        var unitRagDollTransform = Instantiate(unitRagDollPrefab, transform.position, transform.rotation);
        var unitRagDoll = unitRagDollTransform.GetComponent<UnitRagDoll>();
        unitRagDoll.SetUp(boneRoot, args);
    }
}