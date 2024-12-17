using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    private GridPosition gridPosition;
    private bool isRed;
    private Action OnActionComplete;
    private float timer;
    private bool isActive;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(gridPosition, this);

        isRed = false;
        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

         if(timer < 0)
        {
            isActive = false;
            OnActionComplete();
        }
    }

    public void Interact(Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        isActive = true;
        timer = 0.3f;

        if (isRed)
        {
            SetColorGreen();
        }
        else
        {
            SetColorRed();
        }
    }

    private void SetColorGreen()
    {
        isRed = false;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        isRed = true;
        meshRenderer.material = redMaterial;
    }
}
