using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than 1 InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDirection = new Vector2(0f, 0f);

        if (Input.GetKey(KeyCode.W)) inputMoveDirection.x = 1f;
        if (Input.GetKey(KeyCode.S)) inputMoveDirection.x = -1f;
        if (Input.GetKey(KeyCode.A)) inputMoveDirection.y = -1f;
        if (Input.GetKey(KeyCode.D)) inputMoveDirection.y = 1f;

        return inputMoveDirection;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q)) rotateAmount = 1f;
        if (Input.GetKey(KeyCode.E)) rotateAmount = -1f;

        return rotateAmount;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0f) zoomAmount = 1f;
        if (Input.mouseScrollDelta.y < 0f) zoomAmount = -1f;

        return zoomAmount;
    }
}
