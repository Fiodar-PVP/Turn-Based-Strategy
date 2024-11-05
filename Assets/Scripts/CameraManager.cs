using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    // Start is called before the first frame update
    void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideCamera();
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit targetUnit = shootAction.GetTargetUnit();
                Unit shootingUnit = shootAction.GetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - shootingUnit.GetWorldPosition()).normalized;

                float shoulderOffset = 0.5f;
                Vector3 shoulderOffsetPosition = Quaternion.Euler(0f, 90f, 0f) * shootDirection * shoulderOffset;

                Vector3 shootingPosition = shootingUnit.GetWorldPosition() +
                                           cameraCharacterHeight +
                                           shoulderOffsetPosition +
                                           shootDirection * (-1);

                actionCameraGameObject.transform.position = shootingPosition;
                actionCameraGameObject.transform.forward = shootDirection;

                ShowCamera();

                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideCamera();
                break;
        }
    }

    private void ShowCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
