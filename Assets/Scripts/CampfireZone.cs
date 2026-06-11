using UnityEngine;

public class CampfireZone : MonoBehaviour
{
    [Header("Warmth Settings")]
    public float temperatureRestoreRate = 25f;
    public float hungerRestoreRate = 5f;
    
    [Header("Optional Visuals")]
    public GameObject restingText;
    
    private bool playerInZone = false;
    private TemperatureSystem temperatureSystem;
    private HungerSystem hungerSystem;
    
    void Start()
    {
        temperatureSystem = FindObjectOfType<TemperatureSystem>();
        hungerSystem = FindObjectOfType<HungerSystem>();
        
        if (restingText != null)
        {
            restingText.SetActive(false);
        }
    }
    
    void Update()
    {
        if (playerInZone)
        {
            RestoreStats();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            
            if (restingText != null)
            {
                restingText.SetActive(true);
            }
            
            Debug.Log("Resting at campfire...");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            
            if (restingText != null)
            {
                restingText.SetActive(false);
            }
            
            Debug.Log("Left campfire");
        }
    }
    
    void RestoreStats()
    {
        if (temperatureSystem != null)
        {
            temperatureSystem.IncreaseTemperature(temperatureRestoreRate * Time.deltaTime);
        }
        
        if (hungerSystem != null)
        {
            hungerSystem.EatFood(hungerRestoreRate * Time.deltaTime);
        }
    }
}
