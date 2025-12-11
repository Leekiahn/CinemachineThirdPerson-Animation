using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity;

    [Header("Camera Root")]
    [SerializeField] private Transform cameraRoot;

    [Header("Camera Limits")]
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;

    [Header("Zoom Settings")]
    [SerializeField] private CinemachineThirdPersonFollow cinemachineThirdPersonFollow;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoomDistance;
    [SerializeField] private float maxZoomDistance;


    private float rotationX;
    private float rotationY;
    private float targetZoomDistance;



    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 currentRotation = cameraRoot.eulerAngles;
        rotationX = currentRotation.x;
        rotationY = currentRotation.y;

        targetZoomDistance = minZoomDistance;
    }

    private void LateUpdate()
    {
        RotateCamera();
        RotatePlayer();
        HandleZoom();
    }

    private void RotateCamera()
    {
        if(cameraRoot == null) return;

        Vector2 lookInput = inputHandler.LookInput;
        rotationY += lookInput.x * mouseSensitivity;
        rotationX -= lookInput.y * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        cameraRoot.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    private void HandleZoom()
    {
        if (cinemachineThirdPersonFollow == null) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            targetZoomDistance -= scrollInput * zoomSpeed;
            targetZoomDistance = Mathf.Clamp(
                targetZoomDistance,
                minZoomDistance,  // 최소 거리
                maxZoomDistance  // 최대 거리
            );
        }

        cinemachineThirdPersonFollow.CameraDistance = Mathf.Lerp(
            cinemachineThirdPersonFollow.CameraDistance,
            targetZoomDistance,
            Time.deltaTime * zoomSpeed
        );
    }
}
