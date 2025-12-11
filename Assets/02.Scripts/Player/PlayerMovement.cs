using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private new Rigidbody rigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float additionalSprintSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 currentVelocity;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveWithAcceleration();
        Jump();
    }

    /// <summary>
    /// 단순 이동
    /// </summary>
    private void MoveSimple()
    {
        Vector2 dir = inputHandler.MoveInput.normalized;

        if (dir.sqrMagnitude < 0.01f) return;

        Vector3 targetDirection = (transform.forward * dir.y + transform.right * dir.x);

        // 대각선 이동 정규화
        if (targetDirection.sqrMagnitude > 1f)
            targetDirection.Normalize();

        Vector3 move = targetDirection * CurrentSpeed() * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + move);
    }

    /// <summary>
    /// 가속도를 이용한 이동
    /// </summary>
    private void MoveWithAcceleration()
    {
        Vector2 dir = inputHandler.MoveInput;
        Vector3 targetDirection = (transform.forward * dir.y + transform.right * dir.x);

        if (dir.sqrMagnitude > 0.01f)
        {
            // 대각선 이동 정규화
            if (targetDirection.sqrMagnitude > 1f)
                targetDirection.Normalize();

            // 목표 속도
            Vector3 targetVelocity = targetDirection * CurrentSpeed();

            // 가속
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            // 감속
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        // 이동
        if (currentVelocity.sqrMagnitude > 0.001f)
        {
            Vector3 move = currentVelocity * Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + move);
        }
    }

    /// <summary>
    /// 현재 이동 속도 계산
    /// </summary>
    /// <returns>기본 이동 속도 또는 달리기 속도</returns>
    private float CurrentSpeed()
    {
        return inputHandler.SprintInput ? moveSpeed + additionalSprintSpeed : moveSpeed;
    }

    /// <summary>
    /// 점프 처리
    /// </summary>
    private void Jump()
    {
        if(!inputHandler.JumpInput || !IsGrounded()) return;

        //점프 로직
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        inputHandler.JumpInput = false;
    }

    /// <summary>
    /// 지면 체크
    /// </summary>
    /// <returns>지면에 닿아있는지 bool값 반환</returns>
    private bool IsGrounded()
    {
        //지면 체크 로직
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
