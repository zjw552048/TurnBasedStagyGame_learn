using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] private int healthValue = 100;

    public event Action OnHealthZeroAction;

    public void Damage(int damageAmount) {
        healthValue -= damageAmount;
        if (healthValue > 0) {
            return;
        }

        healthValue = 0;
        OnHealthZeroAction?.Invoke();
    }
}