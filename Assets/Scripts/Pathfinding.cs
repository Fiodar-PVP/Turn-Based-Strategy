using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

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

    private void Update()
    {
        //Test PathFinding Implementation
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition startGridPosition = new GridPosition(0, 0);
            GridPosition endGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            List<GridPosition> gridPositionList = FindPath(startGridPosition, endGridPosition);
            
            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.white,
                    10f);
            }
        }
    }

    public void Setup(int width, int height, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(height, width, cellSize,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateGridDebugObject(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        //Clear data from the previous search
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        //Set up the start node
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();
        openList.Add(startNode);

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if(currentNode == endNode)
            {
                //Reached the end node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbour in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbour))
                {
                    //We have already checked that node
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbour.GetGridPosition());

                if(tentativeGCost < neighbour.GetGCost())
                {
                    neighbour.SetGCost(tentativeGCost);
                    neighbour.SetHCost(CalculateDistance(neighbour.GetGridPosition(), endGridPosition));
                    neighbour.CalculateFCost();
                    neighbour.SetCameFromPathNode(currentNode);
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }

        //no path found
        return null;
    }

    private PathNode GetLowestFCostNode(List<PathNode> openList)
    {
        PathNode lowestFCostNode = openList[0];

        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = openList[i];
            }
        }

        return lowestFCostNode;
    }

    private int CalculateDistance(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        GridPosition distance = startGridPosition - endGridPosition;
        int xDistance = Mathf.Abs(distance.x);
        int zDistance = Mathf.Abs(distance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return Mathf.Min(xDistance, zDistance) * MOVE_DIAGONAL_COST + remaining * MOVE_STRAIGHT_COST;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();

        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourNodeList = new List<PathNode>();
        GridPosition currentGridPosition = currentNode.GetGridPosition();

        if(currentGridPosition.x - 1 >= 0)
        {
            if (currentGridPosition.z - 1 >= 0)
            {
                //Left Down
                neighbourNodeList.Add(GetPathNode(currentGridPosition.x - 1, currentGridPosition.z - 1));
            }

            //Left
            neighbourNodeList.Add(GetPathNode(currentGridPosition.x - 1, currentGridPosition.z + 0));

            if (currentGridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighbourNodeList.Add(GetPathNode(currentGridPosition.x - 1, currentGridPosition.z + 1));
            }
        }

        if(currentGridPosition.x + 1 < gridSystem.GetWidth())
        {
            if (currentGridPosition.z - 1 >= 0)
            {   //Right Down
                neighbourNodeList.Add(GetPathNode(currentGridPosition.x + 1, currentGridPosition.z - 1));
            }

            //Right
            neighbourNodeList.Add(GetPathNode(currentGridPosition.x + 1, currentGridPosition.z + 0));

            if (currentGridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighbourNodeList.Add(GetPathNode(currentGridPosition.x + 1, currentGridPosition.z + 1));
            }
        }

        //Up
        if(currentGridPosition.z + 1 < gridSystem.GetHeight())
        {
            neighbourNodeList.Add(GetPathNode(currentGridPosition.x + 0, currentGridPosition.z + 1));
        }

        //Down
        if(currentGridPosition.z - 1 >= 0)
        {
            neighbourNodeList.Add(GetPathNode(currentGridPosition.x + 0, currentGridPosition.z - 1));
        }

        return neighbourNodeList;
    }

    private PathNode GetPathNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
}

