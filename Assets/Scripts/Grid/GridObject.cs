using System.Collections.Generic;

public class GridObject {
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;

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

    public Unit GetUnit() {
        return unitList.Count > 0 ? unitList[0] : null;
    }

    public void SetInteractable(IInteractable value) {
        interactable = value;
    }

    public IInteractable GetInteractable() {
        return interactable;
    }

    public override string ToString() {
        var unitString = "";
        foreach (var unit in unitList) {
            unitString += unit + "\n";
        }

        return $"pos: {gridPosition.ToString()}\n{unitString}";
    }
}