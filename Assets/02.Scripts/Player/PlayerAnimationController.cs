using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerInputHandler inputHandler;
    private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("Animation Settings")]
    [SerializeField] float animationSmoothTime = 0.1f;
    private readonly int hashSpeedX = Animator.StringToHash("SpeedX");
    private readonly int hashSpeedY = Animator.StringToHash("SpeedY");
    private readonly int hashIsSprinting = Animator.StringToHash("IsSprinting");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");
    private readonly int hashVerticalVelocity = Animator.StringToHash("VerticalVelocity");

    private bool wasGrounded = true;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMovementAnimation();
        UpdateJumpAnimation();
    }

    private void UpdateMovementAnimation()
    {
        animator.SetFloat(hashSpeedX, inputHandler.MoveInput.x, animationSmoothTime, Time.deltaTime);
        animator.SetFloat(hashSpeedY, inputHandler.MoveInput.y, animationSmoothTime, Time.deltaTime);
        animator.SetBool(hashIsSprinting, inputHandler.SprintInput);
    }

    private void UpdateJumpAnimation()
    {
        bool isGrounded = playerMovement.IsGrounded();
        float verticalVelocity = rb.linearVelocity.y;

        // 점프
        if (inputHandler.JumpInput && isGrounded)
        {
            animator.SetTrigger(hashJump);
        }

        // 수직 속도
        animator.SetFloat(hashVerticalVelocity, verticalVelocity);

        // 착지
        animator.SetBool(hashIsGrounded, isGrounded);

        // 착지 시점에 점프 트리거 리셋
        if (!wasGrounded && isGrounded)
        {
            animator.ResetTrigger(hashJump);
        }

        // 이전 착지 상태 업데이트
        wasGrounded = isGrounded;
    }
}
