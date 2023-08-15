using UnityEngine;

public class PathNodeUpdateManager : MonoBehaviour {
    private void Start() {
        DestructableCrate.OnAnyCrateDestructedAction += DestructableCrate_OnAnyCrateDestructedAction;
    }

    private void DestructableCrate_OnAnyCrateDestructedAction(GridPosition gridPosition) {
        PathfindingManager.Instance.SetWalkable(gridPosition, true);
    }
}