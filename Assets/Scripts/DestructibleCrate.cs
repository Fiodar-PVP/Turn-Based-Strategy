using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestruction;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        Destroy(gameObject);
        OnAnyCrateDestruction?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition() => gridPosition;
}
