using UnityEngine;
using TMPro;

public class PlayerGathering : MonoBehaviour
{
    [Header("Gathering Settings")]
    public KeyCode gatherKey = KeyCode.E;
    public float gatherRange = 2f;
    public LayerMask gatherableLayer;
    
    [Header("UI References")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI promptText;
    
    [Header("References")]
    private Animator animator;
    private bool isGathering = false;
    private float gatherTimer = 0f;
    private float gatherDuration = 1.5f;
    private GatherableItem currentItem;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        // Hide prompt at start
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    void Update()
    {
        if (isGathering)
        {
            HandleGathering();
            return;
        }
        
        // Check for nearby gatherable items
        GatherableItem nearbyItem = FindNearestGatherableItem();
        
        if (nearbyItem != null && nearbyItem.CanGather())
        {
            // Show prompt
            ShowPrompt(nearbyItem.itemName);
            
            // Check for gather input
            if (Input.GetKeyDown(gatherKey))
            {
                StartGathering(nearbyItem);
            }
        }
        else
        {
            // Hide prompt when not near anything
            HidePrompt();
        }
    }
    
    void ShowPrompt(string itemName)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
            
            if (promptText != null)
            {
                promptText.text = $"E";
            }
        }
    }
    
    void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    GatherableItem FindNearestGatherableItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, gatherRange);
        GatherableItem closestItem = null;
        float closestDistance = gatherRange;
        
        foreach (Collider col in colliders)
        {
            GatherableItem item = col.GetComponent<GatherableItem>();
            if (item != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestItem = item;
                    closestDistance = distance;
                }
            }
        }
        
        return closestItem;
    }
    
    void StartGathering(GatherableItem item)
    {
        isGathering = true;
        currentItem = item;
        gatherTimer = 0f;
        
        // Hide prompt during gathering
        HidePrompt();
        
        // Trigger gathering animation
        if (animator != null)
        {
            animator.SetTrigger("Gather");
        }
        
        Debug.Log("Started gathering...");
    }
    
    void HandleGathering()
    {
        gatherTimer += Time.deltaTime;
        
        if (gatherTimer >= gatherDuration)
        {
            CompleteGathering();
        }
    }
    
    void CompleteGathering()
    {
        if (currentItem != null)
        {
            currentItem.Gather();
        }
        
        isGathering = false;
        currentItem = null;
        gatherTimer = 0f;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gatherRange);
    }
}
