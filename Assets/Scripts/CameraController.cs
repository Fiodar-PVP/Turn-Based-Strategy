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
        Vector3 inputMoveDirection = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W)) inputMoveDirection.x = 1f;
        if (Input.GetKey(KeyCode.S)) inputMoveDirection.x = -1f;
        if (Input.GetKey(KeyCode.A)) inputMoveDirection.z = -1f;
        if (Input.GetKey(KeyCode.D)) inputMoveDirection.z = 1f;

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDirection.x + transform.right * inputMoveDirection.z;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 inputRotationDirection = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.Q)) inputRotationDirection.y = 1f;
        if (Input.GetKey(KeyCode.E)) inputRotationDirection.y = -1f;

        float rotationSpeed = 100f;

        transform.eulerAngles += inputRotationDirection * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0f) targetFollowOffset.y += zoomAmount;
        if (Input.mouseScrollDelta.y < 0f) targetFollowOffset.y -= zoomAmount;

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
