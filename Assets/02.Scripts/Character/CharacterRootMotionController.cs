using UnityEngine;

/// <summary>
/// 캐릭터의 Root Motion 기반 애니메이션 제어를 위한 베이스 클래스
/// </summary>
public abstract class CharacterRootMotionController : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    protected new Rigidbody rigidbody;

    [Header("Movement Settings")]
    [SerializeField] protected float smoothDampTime = 0.1f;

    [Header("Ground Check Settings")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundDistance = 0.2f;
    [SerializeField] protected Transform groundCheck;

    [Header("Attack Settings")]
    [SerializeField] protected CharacterAttack leftHand;
    [SerializeField] protected CharacterAttack rightHand;

    // 애니메이터 해시 (protected로 변경하여 상속 클래스에서 접근 가능)
    protected readonly int hashMoveX = Animator.StringToHash("MoveX");
    protected readonly int hashMoveY = Animator.StringToHash("MoveY");
    protected readonly int hashIsSprinting = Animator.StringToHash("IsSprinting");
    protected readonly int hashDiveRoll = Animator.StringToHash("DiveRoll");
    protected readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");
    protected readonly int hashIsFalling = Animator.StringToHash("IsFalling");
    protected readonly int hashAttack = Animator.StringToHash("Attack");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        UpdateMovement();
        UpdateGroundState();
        UpdateFallingState();
    }

    /// <summary>
    /// 이동 관련 로직 업데이트 (하위 클래스에서 구현)
    /// </summary>
    protected abstract void UpdateMovement();

    /// <summary>
    /// 지상 상태 업데이트
    /// </summary>
    protected virtual void UpdateGroundState()
    {
        animator.SetBool(hashIsGrounded, IsGrounded());
    }

    /// <summary>
    /// 낙하 상태 업데이트
    /// </summary>
    protected virtual void UpdateFallingState()
    {
        if (rigidbody.linearVelocity.y < -0.5f && !IsGrounded())
        {
            animator.SetBool(hashIsFalling, true);
        }
        else
        {
            animator.SetBool(hashIsFalling, false);
        }
    }

    /// <summary>
    /// 지면에 닿아있는지 확인
    /// </summary>
    /// <returns>지면에 닿아있으면 true</returns>
    protected bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    #region Animation Event Handlers - Attack

    /// <summary>
    /// 왼쪽 손 공격 판정 활성화 Animation Event
    /// </summary>
    protected virtual void OnEnableLeftHandAttack()
    {
        if (leftHand != null)
        {
            leftHand.EnableAttack();
        }
    }

    /// <summary>
    /// 왼쪽 손 공격 판정 비활성화 Animation Event
    /// </summary>
    protected virtual void OnDisableLeftHandAttack()
    {
        if (leftHand != null)
        {
            leftHand.DisableAttack();
        }
    }

    /// <summary>
    /// 오른쪽 손 공격 판정 활성화 Animation Event
    /// </summary>
    protected virtual void OnEnableRightHandAttack()
    {
        if (rightHand != null)
        {
            rightHand.EnableAttack();
        }
    }

    /// <summary>
    /// 오른쪽 손 공격 판정 비활성화 Animation Event
    /// </summary>
    protected virtual void OnDisableRightHandAttack()
    {
        if (rightHand != null)
        {
            rightHand.DisableAttack();
        }
    }

    #endregion
}
