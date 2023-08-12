using UnityEngine;

public class PathfindingManager : MonoBehaviour {
    public static PathfindingManager Instance { get; private set; }

    [SerializeField] private Transform debugGridPrefab;

    private GridSystem<PathNode> gridSystem;

    private void Awake() {
        gridSystem = new GridSystem<PathNode>(
            10,
            10,
            2,
            gridPosition => new PathNode(gridPosition));
        gridSystem.CreateDebugGrids(transform, debugGridPrefab);

        Instance = this;
    }
}