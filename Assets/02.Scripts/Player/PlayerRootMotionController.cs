using UnityEngine;

public class PlayerRootMotionController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private Animator animator;
    private new Rigidbody rigidbody;
    private AudioSource audioSource;
    private float smoothDampTime = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private AudioClip[] walkFootStepSound;
    [SerializeField] private AudioClip[] SprintFootStepSound;

    private readonly int hashMoveX = Animator.StringToHash("MoveX");
    private readonly int hashMoveY = Animator.StringToHash("MoveY");
    private readonly int hashIsSprinting = Animator.StringToHash("IsSprinting");
    private readonly int hashDiveRoll = Animator.StringToHash("DiveRoll");
    private readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");

    private bool hasDiveRolled = false;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
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

    private void OnAnimatorMove()
    {
        Vector3 movement = animator.deltaPosition;

        rigidbody.MovePosition(rigidbody.position + movement);
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
}
