using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float addSpinAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0f, addSpinAmount, 0f);
        totalSpinAmount += addSpinAmount;

        if(totalSpinAmount >= 360f) 
        {
            //Unit made the whole circle
            ActionComplete();
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        totalSpinAmount = 0f;

        ActionStart(OnActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();
        return new List<GridPosition> { gridPosition };
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override int GetActionPoinstCost()
    {
        return 1;
    }
}
