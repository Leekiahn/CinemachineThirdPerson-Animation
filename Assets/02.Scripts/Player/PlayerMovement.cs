using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private Rigidbody rigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    private Vector3 currentVelocity;
    public bool useAcceleration = true;



    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(useAcceleration)
        {
            MoveWithAcceleration();
        }
        else
        {
            MoveSimple();
        }
    }

    private void MoveSimple()
    {
        Vector2 dir = inputHandler.MoveInput.normalized;

        if (dir.sqrMagnitude < 0.01f) return;

        Vector3 targetDirection = (transform.forward * dir.y + transform.right * dir.x);

        // 대각선 이동 정규화
        if (targetDirection.sqrMagnitude > 1f)
            targetDirection.Normalize();

        Vector3 move = targetDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + move);
    }

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
            Vector3 targetVelocity = targetDirection * moveSpeed;

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
}
