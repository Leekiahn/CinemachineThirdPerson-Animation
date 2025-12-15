using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput inputAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float ScrollInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool DiveRollInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool AttackInput { get; private set; }

    public bool ESCInput { get; private set; }

    public event Action OnESCInputChanged;

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
        inputAction.Player.DiveRoll.started += OnDiveRollStarted;
        inputAction.Player.DiveRoll.canceled += OnDiveRollCanceled;

        //줌인/줌아웃 이벤트
        inputAction.Player.Zoom.performed += OnScrollPerformed;
        inputAction.Player.Zoom.canceled += OnScrollCanceled;

        //시점 이벤트
        inputAction.Player.Look.performed += OnLookPerformed;
        inputAction.Player.Look.canceled += OnLookCanceled;

        //공격 이벤트
        inputAction.Player.Attack.started += OnAttackPerformed;
        inputAction.Player.Attack.canceled += OnAttackCanceled;

        // ESC 이벤트
        inputAction.Player.ESC.started += OnEscStarted;
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

    private void OnDiveRollStarted(InputAction.CallbackContext context)
    {
        DiveRollInput = true;
    }

    private void OnDiveRollCanceled(InputAction.CallbackContext context)
    {
        DiveRollInput = false;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        LookInput = Vector2.zero;
    }

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        ScrollInput = scrollValue.y;
    }

    private void OnScrollCanceled(InputAction.CallbackContext context)
    {
        ScrollInput = 0f;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AttackInput = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        AttackInput = false;
    }

    private void OnEscStarted(InputAction.CallbackContext context)
    {
        OnESCInputChanged?.Invoke();
    }
}
