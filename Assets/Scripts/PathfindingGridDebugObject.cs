using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;

    private PathNode pathNode;

    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.GetGCost().ToString();
        gCostText.text = pathNode.GetHCost().ToString();
        gCostText.text = pathNode.GetFCost().ToString();
    }

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);

        pathNode = (PathNode)gridObject;
    }
}
