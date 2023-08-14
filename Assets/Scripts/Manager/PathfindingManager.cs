using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour {
    public static PathfindingManager Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private const int RAY_CAST_OFFSET_Y = 5;

    [SerializeField] private Transform debugGridPrefab;
    [SerializeField] private LayerMask obstacleLayerMask;

    private GridSystem<PathNode> gridSystem;

    private int width;
    private int height;
    private float cellSize;

    private void Awake() {
        Instance = this;
    }

    public void SetUp(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(
            width,
            height,
            cellSize,
            gridPosition => new PathNode(gridPosition));
        
        // 注释debugGrid
        // gridSystem.CreateDebugGrids(transform, debugGridPrefab);

        // 利用射线检测整个grid，判断是否存在obstacle导致无法行走
        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var worldPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(x, z));
                if (Physics.Raycast(
                        worldPosition + Vector3.down * RAY_CAST_OFFSET_Y,
                        Vector3.up,
                        RAY_CAST_OFFSET_Y * 2,
                        obstacleLayerMask)) {
                    GetPathNode(x, z).SetWalkable(false);
                }
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            var gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            var pathGridPositionList = FindPath(new GridPosition(0, 0), gridPosition);
            if (pathGridPositionList == null) {
                Debug.Log("Path not found!");
                return;
            }

            for (var index = 0; index < pathGridPositionList.Count - 1; index++) {
                var pathGridPosition = LevelGrid.Instance.GetWorldPosition(pathGridPositionList[index]);
                var nextPathGridPosition = LevelGrid.Instance.GetWorldPosition(pathGridPositionList[index + 1]);
                Debug.DrawLine(pathGridPosition, nextPathGridPosition, Color.red, 10f);
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPos, GridPosition endPos) {
        ResetAllPathNode();

        var openList = new List<PathNode>();
        var closedList = new List<PathNode>();

        var startNode = gridSystem.GetGridObject(startPos);
        var endNode = gridSystem.GetGridObject(endPos);

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPos, endPos));
        startNode.CalculateFCost();

        openList.Add(startNode);

        while (openList.Count > 0) {
            var currentNode = GetLowestFCostNode(openList);
            var currentNodeGridPos = currentNode.getGridPosition();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode) {
                return GetPath(endNode);
            }

            var neighbourNodes = GetNeighbourPathNodeList(currentNode);
            foreach (var neighbourNode in neighbourNodes) {
                if (closedList.Contains(neighbourNode)) {
                    continue;
                }

                if (!neighbourNode.IsWalkable()) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                var neighbourGridPos = neighbourNode.getGridPosition();

                var newGCost = currentNode.GetGCost() + CalculateDistance(currentNodeGridPos, neighbourGridPos);

                if (newGCost < neighbourNode.GetGCost()) {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(newGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourGridPos, endPos));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private void ResetAllPathNode() {
        var width = gridSystem.GetWidth();
        var height = gridSystem.GetHeight();
        for (var x = 0; x < width; x++) {
            for (var z = 0; z < height; z++) {
                var node = gridSystem.GetGridObject(new GridPosition(x, z));
                node.SetGCost(int.MaxValue);
                node.SetHCost(0);
                node.CalculateFCost();
                node.SetCameFromPathNode(null);
            }
        }
    }

    private PathNode GetPathNode(int x, int z) {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private int CalculateDistance(GridPosition fromPos, GridPosition toPos) {
        var movementVector = fromPos - toPos;
        var xDistance = Mathf.Abs(movementVector.x);
        var zDistance = Mathf.Abs(movementVector.z);
        var straightDistance = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * straightDistance;
    }

    private PathNode GetLowestFCostNode(List<PathNode> openList) {
        var lowestFCostNode = openList[0];
        foreach (var node in openList) {
            if (node.GetFCost() < lowestFCostNode.GetFCost()) {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    private List<GridPosition> GetPath(PathNode endNode) {
        var gridPositionList = new List<GridPosition>();
        var currenNode = endNode;

        do {
            gridPositionList.Add(currenNode.getGridPosition());
            currenNode = currenNode.GetCameFromPathNode();
        } while (currenNode != null);

        gridPositionList.Reverse();

        return gridPositionList;
    }

    private List<PathNode> GetNeighbourPathNodeList(PathNode pathNode) {
        var width = gridSystem.GetWidth();
        var height = gridSystem.GetHeight();

        var gridPosition = pathNode.getGridPosition();
        var neighbourPathNodeList = new List<PathNode>();

        if (gridPosition.x > 0) {
            // left
            neighbourPathNodeList.Add(GetPathNode(gridPosition.x - 1, gridPosition.z));

            if (gridPosition.z < height) {
                // left up
                neighbourPathNodeList.Add(GetPathNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if (gridPosition.z > 0) {
                // left down
                neighbourPathNodeList.Add(GetPathNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.x < width) {
            // right
            neighbourPathNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z));

            if (gridPosition.z < height) {
                // right up
                neighbourPathNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z + 1));
            }

            if (gridPosition.z > 0) {
                // right down
                neighbourPathNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.z < height) {
            // up
            neighbourPathNodeList.Add(GetPathNode(gridPosition.x, gridPosition.z + 1));
        }

        if (gridPosition.z > 0) {
            // down
            neighbourPathNodeList.Add(GetPathNode(gridPosition.x, gridPosition.z - 1));
        }


        return neighbourPathNodeList;
    }
}