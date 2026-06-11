using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public float groundCheckDistance = 0.2f;
    
    [Header("References")]
    public Transform cameraTransform;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    private StaminaSystem staminaSystem;
    private AudioManager audioManager;
    private TerrainDetector terrainDetector;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        staminaSystem = FindObjectOfType<StaminaSystem>();
        audioManager = FindObjectOfType<AudioManager>();
        terrainDetector = GetComponent<TerrainDetector>();
        
        if (terrainDetector == null)
        {
            terrainDetector = gameObject.AddComponent<TerrainDetector>();
        }
        
        if (controller == null)
        {
            Debug.LogError("No Character Controller found on " + gameObject.name);
        }
        
        if (cameraTransform == null)
        {
            GameObject cameraFollower = GameObject.Find("CameraFollower");
            if (cameraFollower != null)
                cameraTransform = cameraFollower.transform;
        }
    }
    
    void Update()
    {
        if (controller == null || cameraTransform == null) return;
        
        isGrounded = controller.isGrounded;
        
        if (!isGrounded)
        {
            RaycastHit hit;
            float rayDistance = 0.3f;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance);
        }
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (staminaSystem != null && staminaSystem.CanJump())
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                staminaSystem.UseJump();
                AudioManager.PlayJumpSound();
            }
            else if (staminaSystem == null)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                AudioManager.PlayJumpSound();
            }
        }
        
        float currentSpeed = 0f;
        bool isSprinting = false;
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            if (Input.GetKey(KeyCode.LeftShift) && staminaSystem != null && staminaSystem.CanSprint())
            {
                currentSpeed = runSpeed;
                isSprinting = true;
                staminaSystem.UseSprint(Time.deltaTime);
            }
            else
            {
                currentSpeed = walkSpeed;
            }
            
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
            
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            if (audioManager != null && isGrounded)
            {
                string terrainType = terrainDetector != null ? terrainDetector.GetCurrentTerrain(transform.position) : "grass";
                audioManager.PlayFootsteps(terrainType);
            }
        }
        else
        {
            if (audioManager != null)
            {
                audioManager.StopFootsteps();
            }
        }
        
        if (!isSprinting && staminaSystem != null)
        {
            staminaSystem.RegenerateStamina(Time.deltaTime);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("YVelocity", velocity.y);
        }
    }
}
