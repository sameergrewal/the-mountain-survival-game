using UnityEngine;

public class SimpleAnimalAI : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1f;
    public float turnSpeed = 1f;
    public float maxSlopeAngle = 35f;
    
    [Header("Behavior")]
    public float minWalkTime = 4f;
    public float maxWalkTime = 8f;
    public float minIdleTime = 3f;
    public float maxIdleTime = 7f;
    public float directionChangeChance = 0.3f;
    
    [Header("Ground Detection")]
    public float groundCheckDistance = 2f;
    
    private Animator animator;
    private float walkTimer = 0f;
    private float idleTimer = 0f;
    private bool isWalking = false;
    private Vector3 targetDirection;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
    }
    
    void Update()
    {
        if (isWalking)
        {
            HandleWalking();
        }
        else
        {
            HandleIdle();
        }
        
        StickToGround();
    }
    
    void HandleWalking()
    {
        walkTimer -= Time.deltaTime;
        
        if (Random.value < directionChangeChance * Time.deltaTime)
        {
            AdjustDirection();
        }
        
        if (!CanMoveInDirection(transform.forward))
        {
            PickNewDirection();
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        
        Vector3 movement = transform.forward * walkSpeed * Time.deltaTime;
        transform.position += movement;
        
        if (walkTimer <= 0f)
        {
            StopWalking();
        }
    }
    
    void HandleIdle()
    {
        idleTimer -= Time.deltaTime;
        
        if (Random.value < 0.1f * Time.deltaTime)
        {
            float randomTurn = Random.Range(-30f, 30f);
            transform.Rotate(0, randomTurn, 0);
        }
        
        if (idleTimer <= 0f)
        {
            StartWalking();
        }
    }
    
    void StartWalking()
    {
        isWalking = true;
        walkTimer = Random.Range(minWalkTime, maxWalkTime);
        PickNewDirection();
        
        if (animator != null)
        {
            animator.SetFloat("Speed", walkSpeed);
        }
    }
    
    void StopWalking()
    {
        isWalking = false;
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
        
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    
    void PickNewDirection()
    {
        for (int i = 0; i < 8; i++)
        {
            float randomAngle = Random.Range(-120f, 120f);
            Vector3 testDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward;
            
            if (CanMoveInDirection(testDirection))
            {
                targetDirection = testDirection;
                return;
            }
        }
        
        targetDirection = -transform.forward;
    }
    
    void AdjustDirection()
    {
        float adjustment = Random.Range(-45f, 45f);
        targetDirection = Quaternion.Euler(0, adjustment, 0) * targetDirection;
        targetDirection.Normalize();
    }
    
    bool CanMoveInDirection(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 checkPosition = rayOrigin + direction.normalized * 1f;
        
        if (Physics.Raycast(checkPosition, Vector3.down, out hit, groundCheckDistance))
        {
            float slope = Vector3.Angle(hit.normal, Vector3.up);
            
            if (slope > maxSlopeAngle)
            {
                return false;
            }
            
            float heightDifference = hit.point.y - transform.position.y;
            if (Mathf.Abs(heightDifference) > 1.5f)
            {
                return false;
            }
            
            return true;
        }
        
        return false;
    }
    
    void StickToGround()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y;
            transform.position = newPosition;
        }
    }
}
