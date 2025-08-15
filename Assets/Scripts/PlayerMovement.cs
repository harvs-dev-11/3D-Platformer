using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Animator animator;
    
    private PlayerControls controls;
    private Vector2 moveInput;
    private CharacterController controller;
    
    private float verticalVelocity;
    private bool jumpPressed;
    private float smoothedSpeed;
    private bool jumpRequest;
    private bool jumpConsumedThisGrounded;
    private bool wasGrounded;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx =>
        {
            if (controller.isGrounded && !jumpConsumedThisGrounded)
                jumpRequest = true;
        };
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleJump();
        UpdateAnimator();
        HandleJumpConsumed();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(-moveInput.y, 0, moveInput.x);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        move *= moveSpeed;
        move.y = verticalVelocity;
        
        controller.Move(move * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (jumpRequest)
            {
                jumpRequest = false;
                jumpConsumedThisGrounded = true;
                animator.SetTrigger("Jump");
            }
        }
    }

    private void HandleJumpConsumed()
    {
        bool grounded = controller.isGrounded;
        
        if (!wasGrounded && grounded)
            jumpConsumedThisGrounded = false;
        
        if (wasGrounded && !grounded)
            jumpRequest = false;

        wasGrounded = grounded;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetFloat("VerticalVelocity", verticalVelocity);

        float targetSpeed = Mathf.Clamp01(moveInput.magnitude);
        smoothedSpeed = Mathf.MoveTowards(smoothedSpeed, targetSpeed, Time.deltaTime * 5f);

        animator.SetFloat("Speed", smoothedSpeed);
    }
    
    public void OnJumpPushOff()
    {
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}