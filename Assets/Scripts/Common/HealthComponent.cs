using System;
using Enum;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] private int maxHealthValue = 100;
    private int healthValue;

    public event EventHandler<OnHealthZeroActionArgs> OnHealthZeroAction;

    public class OnHealthZeroActionArgs : EventArgs {
        public DamageType damageType;
        public int damageAmount;
        public Vector3 damageVector;
        public float damageRadius;
    }

    public event Action OnHealthChangedAction;

    private void Awake() {
        healthValue = maxHealthValue;
    }

    public void ShootDamage(int damageAmount, Vector3 damageDir) {
        healthValue -= damageAmount;
        OnHealthChangedAction?.Invoke();
        if (healthValue > 0) {
            return;
        }

        healthValue = 0;
        OnHealthZeroAction?.Invoke(this, new OnHealthZeroActionArgs {
                damageType = DamageType.ShootDamage,
                damageAmount = damageAmount,
                damageVector = damageDir,
                damageRadius = 0
            }
        );
    }

    public void GrenadeDamage(int damageAmount, Vector3 damageCenter, float damageRadius) {
        healthValue -= damageAmount;
        OnHealthChangedAction?.Invoke();
        if (healthValue > 0) {
            return;
        }

        healthValue = 0;
        OnHealthZeroAction?.Invoke(this, new OnHealthZeroActionArgs {
            damageType = DamageType.GrenadeDamage,
            damageAmount = damageAmount,
            damageVector = damageCenter,
            damageRadius = damageRadius
        });
    }

    public float GetHealthNormalized() {
        return (float) healthValue / maxHealthValue;
    }
}