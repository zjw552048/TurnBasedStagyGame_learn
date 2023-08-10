using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    protected Unit unit;
    protected Transform selfTransform;
    protected bool actionActive;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
        selfTransform = unit.transform;
    }
}