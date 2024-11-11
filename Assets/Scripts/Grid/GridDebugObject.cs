using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private object gridObject;

    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }

    public void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }
}
