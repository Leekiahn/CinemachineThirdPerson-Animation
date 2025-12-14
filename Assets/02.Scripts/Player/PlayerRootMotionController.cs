using UnityEngine;

/// <summary>
/// 플레이어 Root Motion 애니메이션 컨트롤러
/// </summary>
public class PlayerRootMotionController : CharacterRootMotionController
{
    private PlayerInputHandler inputHandler;

    [Header("Player Audio")]
    [SerializeField] private AudioData playerAudioData;

    // 다이브 롤 중복 방지 변수
    private bool hasDiveRolled = false;
    private bool hasAttacked = false;

    protected override void Awake()
    {
        base.Awake();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    /// <summary>
    /// 플레이어 입력에 따른 이동 처리
    /// </summary>
    protected override void UpdateMovement()
    {
        // 이동 입력 처리
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

        // 공격 입력 처리
        if (inputHandler.AttackInput && IsGrounded() && !hasAttacked)
        {
            animator.SetTrigger(hashAttack);
            hasAttacked = true;
        }
        if (!inputHandler.AttackInput)
        {
            hasAttacked = false;
        }
    }

    #region Animation Event Handlers - Audio

    /// <summary>
    /// 걷기 발소리 재생 Animation Event
    /// </summary>
    private void OnWalkFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.walkFootStepSound));
        }
    }

    /// <summary>
    /// 달리기 발소리 재생 Animation Event
    /// </summary>
    private void OnSprintFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.sprintFootStepSound));
        }
    }

    /// <summary>
    /// 다이브 롤 발소리 재생 Animation Event
    /// </summary>
    private void OnDiveRollFootStep()
    {
        if (IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.diveRollFootStepSound));
        }
    }

    /// <summary>
    /// 다이브 롤 음성 재생 Animation Event
    /// </summary>
    private void OnDiveRollVoice()
    {
        if (IsGrounded())
        {
            audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.diveRollVoice));
        }
    }

    /// <summary>
    /// 착지 발소리 재생 Animation Event
    /// </summary>
    private void OnLandFootStep()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.landFootStepSound));
    }

    /// <summary>
    /// 착지 음성 재생 Animation Event
    /// </summary>
    private void OnLandVoice()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.landVoice));
    }

    /// <summary>
    /// 공격 음성 재생 Animation Event
    /// </summary>
    private void OnAttackVoice()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.attackVoice));
    }

    private void OnHitVoice()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.hitVoice));
    }

    private void OnDeadVoice()
    {
        audioSource.PlayOneShot(playerAudioData.GetRandomClip(playerAudioData.deadVoice));
    }

    #endregion
}
