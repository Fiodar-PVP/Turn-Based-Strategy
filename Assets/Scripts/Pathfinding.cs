using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than 1 Pathfinding system! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void Setup(int width, int height, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(height, width, cellSize,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateGridDebugObject(gridDebugObjectPrefab);
    }
}

