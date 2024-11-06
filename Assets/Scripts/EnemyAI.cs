using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            timer = 2f;

            state = State.TakingTurn;
        }
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        state = State.WaitingForEnemyTurn;
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy: 
                break;
        }
    }

    private bool TryTakeEnemyAIAction(Action OnEnemyAIActionCompleted)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()) 
        {
            if (TryTakeEnemyAIAction(enemyUnit, OnEnemyAIActionCompleted))
            {
                return true;       
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action OnEnemyAIActionCompleted)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition gridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(gridPosition))
        {
            return false;
        }

        if (!enemyUnit.TryToSpendActionPoints(spinAction))
        {
            return false;
        }

        spinAction.TakeAction(gridPosition, OnEnemyAIActionCompleted);

        return true;
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;

        state = State.TakingTurn;
    }
}
