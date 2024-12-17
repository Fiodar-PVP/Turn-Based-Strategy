using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private const string IS_OPEN = "IsOpen";

    [SerializeField] private bool isOpen = false;

    private GridPosition gridPosition;
    private Animator animator;
    private Action OnActionComplete;
    private float timer;
    private bool isActive;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(gridPosition, this);

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
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
        timer = 0.5f;
        isActive = true;


        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool(IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
