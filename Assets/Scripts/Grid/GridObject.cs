using System.Collections.Generic;

/// <summary>
/// Represents an object within a grid system, which is instantiated in the each grid position and contains varios information about that "cell".
/// </summary>
public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        unitList = new List<Unit>();
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += "\n " + unit;
        }

        return gridPosition.ToString() + unitString;
    }
}