using System.Collections.Generic;

public class GridObject {
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public List<Unit> GetUnitList() {
        return unitList;
    }

    public void AddUnit(Unit unit) {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) {
        unitList.Remove(unit);
    }

    public override string ToString() {
        var unitString = "";
        foreach (var unit in unitList) {
            unitString += unit + "\n";
        }

        return $"pos: {gridPosition.ToString()}\n{unitString}";
    }
}