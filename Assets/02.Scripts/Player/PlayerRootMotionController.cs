using UnityEngine;

public class PlayerRootMotionController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private Animator animator;
    private AudioSource audioSource;
    private new Rigidbody rigidbody;

    [Header("Settings")]
    [SerializeField] private float smoothDampTime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private PlayerAudioData playerAudioData;

    // 애니메이터 해시
    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashIsSprinting = Animator.StringToHash("IsSprinting");
    private readonly int hashDiveRoll = Animator.StringToHash("DiveRoll");
    private readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");
    private readonly int hashIsFalling = Animator.StringToHash("IsFalling");

    // 다이브 롤 중복 방지 변수
    private bool hasDiveRolled = false;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        animator.SetFloat(hashMoveX, inputHandler.MoveInput.x, smoothDampTime, Time.deltaTime);
        animator.SetFloat(hashMoveY, inputHandler.MoveInput.y, smoothDampTime, Time.deltaTime);
        animator.SetBool(hashIsSprinting, inputHandler.SprintInput);

        // 다이브 롤 입력 처리
        if (inputHandler.DiveRollInput && IsGrounded() && !hasDiveRolled)
        {
            animator.SetTrigger(hashDiveRoll);
            hasDiveRolled = true;
        }
        if (!inputHandler.DiveRollInput)
        {
            hasDiveRolled = false;
        }

        // 낙하 상태 처리
        if (rigidbody.linearVelocity.y < -0.5f && !IsGrounded())
        {
            animator.SetBool(hashIsFalling, true);
        }
        else
        {
            animator.SetBool(hashIsFalling, false);
        }

        // 지상 상태 처리
        animator.SetBool(hashIsGrounded, IsGrounded());
    }

    /// <summary>
    /// 지면에 닿아있는지 확인
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    private void OnWalkFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.walkFootStepSound));
        }
    }

    private void OnSprintFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.sprintFootStepSound));
        }
    }

    private void OnDiveRollFootStep()
    {
        if (IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.diveRollFootStepSound));
        }
    }

    private void OnDiveRollVoice()
    {
        if (IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.diveRollVoice));
        }
    }

    private void OnLandFootStep()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.landFootStepSound));
    }

    private void OnLandVoice()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.landVoice));
    }
}
