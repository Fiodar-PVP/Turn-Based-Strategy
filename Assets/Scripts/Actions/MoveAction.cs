using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int maxMoveDistance = 2;

    private Vector3 targetPosition;

    public bool IsWalking { get; private set; }

    protected override void Awake()
    {
        targetPosition = transform.position;

        base.Awake();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float stoppingDistance = 0.01f;
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            IsWalking = true;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }
        else
        {
            IsWalking = false;
            isActive = false;
            OnActionComplete();
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Not valid GridPosition
                    continue;
                }

                if(testGridPosition == unitGridPosition)
                {
                    //Unit is already located in that GridPosition
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition is already occupied by another unit
                    continue;
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }

        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        isActive = true;
        this.OnActionComplete = OnActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
