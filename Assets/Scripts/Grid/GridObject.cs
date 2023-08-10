using System.Collections.Generic;

public class GridObject {
    private GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridPosition gridPosition) {
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

    public bool HasAnyUnit() {
        return unitList.Count > 0;
    }

    public override string ToString() {
        var unitString = "";
        foreach (var unit in unitList) {
            unitString += unit + "\n";
        }

        return $"pos: {gridPosition.ToString()}\n{unitString}";
    }
}