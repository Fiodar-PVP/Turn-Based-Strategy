using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;

    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);

        pathNode = (PathNode)gridObject;
    }
}
