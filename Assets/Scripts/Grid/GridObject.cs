public class GridObject {
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private Unit unit;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public Unit GetUnit() {
        return unit;
    }

    public void SetUnit(Unit unit) {
        this.unit = unit;
    }

    public override string ToString() {
        return $"pos: {gridPosition.ToString()}\nUnit: {unit.name}";
    }
}