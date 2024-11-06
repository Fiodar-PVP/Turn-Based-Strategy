using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 2;

    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDied;
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;
    
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private int currentActionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseActionArray = GetComponentsInChildren<BaseAction>();

        healthSystem.OnDie += HealthSystem_OnDie;
    }


    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedToGridPosition(this, oldGridPosition, newGridPosition);
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

    public ShootAction GetShootAction()
    {
        return shootAction;
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

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
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

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(this, gridPosition);

        Destroy(gameObject);

        OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
    }

    public float GetNormalizedHealth() => healthSystem.GetHealthNormalized();
}
