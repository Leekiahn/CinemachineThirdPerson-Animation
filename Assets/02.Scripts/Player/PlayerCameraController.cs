using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;


    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform cameraPosition;


    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        Vector2 lookInput = inputHandler.LookInput;
        if (lookInput.sqrMagnitude < 0.01f) return;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up, mouseX);
        cameraPosition.Rotate(Vector3.right, -mouseY);
    }
}
