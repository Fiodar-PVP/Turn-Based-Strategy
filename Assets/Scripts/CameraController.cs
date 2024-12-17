using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const int MIN_FOLLOW_Y_OFFSET = 2;
    private const int MAX_FOLLOW_Y_OFFSET = 12;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;

    private void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0f, 0f, 0f);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount();
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;

        float stoppingDifference = 0.01f;
        float currentDifference = Vector3.Distance(cinemachineTransposer.m_FollowOffset, targetFollowOffset);
        if( currentDifference > stoppingDifference)
        {
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
        }
    }
}
