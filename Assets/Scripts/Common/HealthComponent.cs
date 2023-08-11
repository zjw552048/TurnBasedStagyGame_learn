using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] private int maxHealthValue = 100;
    private int healthValue;

    public event Action<Vector3> OnHealthZeroAction;
    public event Action OnHealthChangedAction;

    private void Awake() {
        healthValue = maxHealthValue;
    }

    public void Damage(int damageAmount, Vector3 damageDir) {
        healthValue -= damageAmount;
        OnHealthChangedAction?.Invoke();
        if (healthValue > 0) {
            return;
        }

        healthValue = 0;
        OnHealthZeroAction?.Invoke(damageAmount * damageDir);
    }

    public float GetHealthNormalized() {
        return (float) healthValue / maxHealthValue;
    }
}