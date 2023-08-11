using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] private int healthValue = 100;

    public event Action<Vector3> OnHealthZeroAction;

    public void Damage(int damageAmount, Vector3 damageDir) {
        healthValue -= damageAmount;
        if (healthValue > 0) {
            return;
        }

        healthValue = 0;
        OnHealthZeroAction?.Invoke(damageAmount * damageDir);
    }
}