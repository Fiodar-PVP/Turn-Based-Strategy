using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }
    
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Instance.mousePlaneLayerMask);
        return hitInfo.point;
    }
}
