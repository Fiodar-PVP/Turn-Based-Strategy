using System;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int height;
    private int width;
    private float cellSize;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int height, int width, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, 0, gridPosition.z * cellSize);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) 
    {
        return new GridPosition
        {
            x = Mathf.RoundToInt(worldPosition.x / cellSize),
            z = Mathf.RoundToInt(worldPosition.z / cellSize)
        };
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public void CreateGridDebugObject(Transform prefabTransform)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform debugTransform = GameObject.Instantiate(prefabTransform, GetWorldPosition(gridPosition), Quaternion.identity);

                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
