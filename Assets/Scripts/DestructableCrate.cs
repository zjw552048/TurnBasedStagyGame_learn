using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour {
    
    public static event Action<GridPosition> OnAnyCrateDestructedAction;

    public void Damage() {
        var gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        OnAnyCrateDestructedAction?.Invoke(gridPosition);
        
        Destroy(gameObject);
    }
}