using System;
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
            isActive = false;
            OnActionComplete();
        }

    }

    public void Spin(Action OnActionComplete)
    {
        totalSpinAmount = 0f;
        isActive = true;
        this.OnActionComplete = OnActionComplete;
    }
    public override string GetActionName()
    {
        return "Spin";
    }
}
