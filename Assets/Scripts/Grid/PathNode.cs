public class PathNode {
    private readonly GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool walkable = true;

    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
    }

    public override string ToString() {
        return gridPosition.ToString();
    }

    public GridPosition getGridPosition() {
        return gridPosition;
    }

    public int GetGCost() {
        return gCost;
    }

    public void SetGCost(int value) {
        gCost = value;
    }

    public int GetHCost() {
        return hCost;
    }

    public void SetHCost(int value) {
        hCost = value;
    }

    public int GetFCost() {
        return fCost;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public PathNode GetCameFromPathNode() {
        return cameFromPathNode;
    }

    public void SetCameFromPathNode(PathNode pathNode) {
        cameFromPathNode = pathNode;
    }

    public bool IsWalkable() {
        return walkable;
    }

    public void SetWalkable(bool value) {
        walkable = value;
    }
}