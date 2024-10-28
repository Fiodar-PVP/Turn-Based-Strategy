using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;
    
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private int currentActionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponentsInChildren<BaseAction>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedToGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (isEnemy && !TurnSystem.Instance.IsPlayerTurn() ||
            !isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {
            ResetActionPoints();
        }
    }


    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public void ResetActionPoints()
    {
        currentActionPoints = MAX_ACTION_POINTS;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return currentActionPoints >= baseAction.GetActionPoinstCost();
    }

    private void SpendActionPoints(int amoint)
    {
        currentActionPoints -= amoint;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TryToSpendActionPoints(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPoinstCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetActionPoints()
    {
        return currentActionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage()
    {
        //For testing purposes
        Debug.Log(transform + " Damaged!");
    }
}
