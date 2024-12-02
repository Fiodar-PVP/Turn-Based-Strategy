using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyCrateDestruction += DestructibleCrate_OnAnyCrateDestruction;
    }

    private void DestructibleCrate_OnAnyCrateDestruction(object sender, System.EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
