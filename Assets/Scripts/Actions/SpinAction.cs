using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : MonoBehaviour
{
    private bool isActive;
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
        }

    }

    public void Spin()
    {
        totalSpinAmount = 0f;
        isActive = true;
    }
}
