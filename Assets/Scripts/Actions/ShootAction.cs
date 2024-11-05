using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [SerializeField] private int maxShootDistance = 7;
    
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break; 
            case State.Shooting:
                if (canShootBullet)
                {
                    canShootBullet = false;
                    Shoot();
                }
                break; 
            case State.Cooloff:
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
            case State.Aiming:
                float shootingStateTimer = 0.1f;
                stateTimer = shootingStateTimer;
                state = State.Shooting;
                break;
            case State.Shooting:
                float cooloffStateTimer = 0.5f;
                stateTimer = cooloffStateTimer;
                state = State.Cooloff;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        }) ;

        int damageAmount = 40;
        targetUnit.Damage(damageAmount);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Not valid GridPosition
                    continue;
                }

                int testtDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testtDistance > maxShootDistance)
                {
                    //Make action work within fixed radius (use circle instead of square)
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //GridPosition is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy())
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

        float aimingStateTimer = 1f;
        stateTimer = aimingStateTimer;
        state = State.Aiming;

        canShootBullet = true;

        ActionStart(OnActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
}
