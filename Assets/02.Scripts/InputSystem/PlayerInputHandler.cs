using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput inputAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool JumpInput { get; set; }
    public bool CrouchInput { get; private set; }

    private void Awake()
    {
        inputAction = new PlayerInput();

        //이동 이벤트
        inputAction.Player.Move.performed += OnMovePerformed;
        inputAction.Player.Move.canceled += OnMoveCanceled;

        // Sprint 이벤트
        inputAction.Player.Sprint.performed += OnSprintPerformed;
        inputAction.Player.Sprint.canceled += OnSprintCanceled;

        // 점프 이벤트
        inputAction.Player.Jump.started += OnJumpStarted;
        inputAction.Player.Jump.canceled += OnJumpCanceled;

        // 웅크리기 이벤트
        inputAction.Player.Crouch.performed += OnCrouchPerformed;
        inputAction.Player.Crouch.canceled += OnCrouchCanceled;

        //시점 이벤트
        inputAction.Player.Look.performed += OnLookPerformed;
        inputAction.Player.Look.canceled += OnLookCanceled;
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

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        SprintInput = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        SprintInput = false;
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        JumpInput = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        JumpInput = false;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        CrouchInput = true;
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        CrouchInput = false;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        LookInput = Vector2.zero;
    }
}
