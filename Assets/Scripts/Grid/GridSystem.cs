using UnityEngine;

public class GridSystem
{
    private int height;
    private int width;
    private float cellSize;
    private GridObject[,] gridObjectArray;

    public GridSystem(int height, int width, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
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

    public GridObject GetGridObject(GridPosition gridPosition)
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

}
