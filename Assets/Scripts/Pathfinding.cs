using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

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
        //gridSystem.CreateGridDebugObject(gridDebugObjectPrefab);

        for(int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for(int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if(Physics.Raycast(worldPosition - Vector3.up * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask))
                {
                    GetPathNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
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
                pathLength = endNode.GetFCost();
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

                if (!neighbour.IsWalkable())
                {
                    closedList.Add(neighbour);
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
        pathLength = 0;
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

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return GetPathNode(gridPosition.x, gridPosition.z).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null; 
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}

