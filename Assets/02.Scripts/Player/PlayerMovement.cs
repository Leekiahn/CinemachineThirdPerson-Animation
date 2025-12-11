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
    private Vector3 currentVelocity;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveWithAcceleration();
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
}
