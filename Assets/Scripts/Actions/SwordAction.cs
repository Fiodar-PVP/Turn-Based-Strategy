using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static EventHandler OnAnySwordHit;

    public event EventHandler OnSwordSlashStarted;
    public event EventHandler OnSwordSlashCompleted;

    private enum State
    {
        SwingingSwordBeforHit,
        SwingingSwordAfterHit
    }

    private Unit targetUnit;
    private State state;
    private float stateTimer;
    private int maxSlashDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer < 0)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforHit:
                float afterHitStateTimer = 0.5f;
                stateTimer = afterHitStateTimer;
                state = State.SwingingSwordAfterHit;

                int damageAmount = 100;
                targetUnit.Damage(damageAmount);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);

                break;
            case State.SwingingSwordAfterHit:
                OnSwordSlashCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSlashDistance; x <= maxSlashDistance; x++)
        {
            for (int z = -maxSlashDistance; z <= maxSlashDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Not valid GridPosition
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Target Unit belongs to the "same" team
                    continue;
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }

        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        float beforeHitStateTimer = 0.7f;
        stateTimer = beforeHitStateTimer;
        state = State.SwingingSwordBeforHit;

        OnSwordSlashStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(OnActionComplete);
    }

    public int GetMaxSlashDistance() => maxSlashDistance;
}
