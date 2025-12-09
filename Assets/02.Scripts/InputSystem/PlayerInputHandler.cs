using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput inputAction;

    public event Action<Vector2> OnMoveInput;

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        inputAction = new PlayerInput();

        // event subscriptions
        inputAction.Player.Move.started += OnMoveStarted;
        inputAction.Player.Move.performed += OnMovePerformed;
        inputAction.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
    }

    private void OnDisable()
    {
        inputAction.Player.Disable();
    }

    private void OnDestroy()
    {
        inputAction.Dispose();
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        Debug.Log("Move started");
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move performed: {MoveInput}");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
        Debug.Log("Move canceled");
    }
}
