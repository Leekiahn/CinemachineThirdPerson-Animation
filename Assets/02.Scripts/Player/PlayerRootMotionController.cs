using UnityEngine;

public class PlayerRootMotionController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private AudioClip[] walkFootStepSound;
    [SerializeField] private AudioClip[] SprintFootStepSound;
    [SerializeField] private AudioClip[] DiveRollFootStepSound;
    [SerializeField] private AudioClip[] DiveRollVoice;
    [SerializeField] private AudioClip[] LandVoice;
    [SerializeField] private AudioClip[] LandFootStepSound;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashIsSprinting = Animator.StringToHash("IsSprinting");
    private readonly int hashDiveRoll = Animator.StringToHash("DiveRoll");
    private readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");
    private float smoothDampTime = 0.1f;

    private bool hasDiveRolled = false;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        animator.SetFloat(hashMoveX, inputHandler.MoveInput.x, smoothDampTime, Time.deltaTime);
        animator.SetFloat(hashMoveY, inputHandler.MoveInput.y, smoothDampTime, Time.deltaTime);
        animator.SetBool(hashIsSprinting, inputHandler.SprintInput);

        if(inputHandler.DiveRollInput && IsGrounded() && !hasDiveRolled)
        {
            animator.SetTrigger(hashDiveRoll);
            hasDiveRolled = true;
        }
        if(!inputHandler.DiveRollInput)
        {
            hasDiveRolled = false;
        }


        animator.SetBool(hashIsGrounded, IsGrounded());
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    private void OnWalkFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            int index = Random.Range(0, walkFootStepSound.Length);
            audioSource.PlayOneShot(walkFootStepSound[index]);
        }
    }

    private void OnSprintFootStep()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f && IsGrounded())
        {
            int index = Random.Range(0, SprintFootStepSound.Length);
            audioSource.PlayOneShot(SprintFootStepSound[index]);
        }
    }

    private void OnDiveRollFootStep()
    {
        if (IsGrounded())
        {
            int index = Random.Range(0, DiveRollFootStepSound.Length);
            audioSource.PlayOneShot(DiveRollFootStepSound[index]);
        }
    }

    private void OnDiveRollVoice()
    {
        if(IsGrounded())
        {
            int index = Random.Range(0, DiveRollVoice.Length);
            audioSource.PlayOneShot(DiveRollVoice[index]);
        }
    }

    private void OnLandFootStep()
    {
        int index = Random.Range(0, LandFootStepSound.Length);
        audioSource.PlayOneShot(LandFootStepSound[index]);
    }

    private void OnLandVoice()
    {
        int index = Random.Range(0, LandVoice.Length);
        audioSource.PlayOneShot(LandVoice[index]);
    }
}
