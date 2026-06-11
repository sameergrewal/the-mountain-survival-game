using UnityEngine;

public class GatherableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName = "Berry";
    public ItemType itemType = ItemType.Food;
    
    [Header("Item Values")]
    public float hungerRestored = 20f;
    public float temperatureRestored = 0f;
    
    [Header("Visual Settings")]
    public GameObject fullBush;
    public GameObject emptyBush;
    public float respawnTime = 30f;
    
    private bool isGathered = false;
    private float respawnTimer = 0f;
    
    public enum ItemType
    {
        Food,
        Clothing,
        SmallAnimal
    }
    
    void Update()
    {
        if (isGathered && respawnTime > 0)
        {
            respawnTimer += Time.deltaTime;
            
            if (respawnTimer >= respawnTime)
            {
                RegrowBerries();
            }
        }
    }
    
    public bool CanGather()
    {
        return !isGathered;
    }
    
    public void Gather()
    {
        if (isGathered) return;
        
        isGathered = true;
        
        if (fullBush != null && emptyBush != null)
        {
            fullBush.SetActive(false);
            emptyBush.SetActive(true);
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            
            if (respawnTime <= 0)
            {
                Destroy(gameObject, 2f);
            }
        }
        
        ApplyItemEffects();
        
        Debug.Log($"Gathered: {itemName}");
        
        respawnTimer = 0f;
    }
    
    void RegrowBerries()
    {
        isGathered = false;
        respawnTimer = 0f;
        
        if (fullBush != null && emptyBush != null)
        {
            fullBush.SetActive(true);
            emptyBush.SetActive(false);
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        Debug.Log($"{itemName} has respawned!");
    }
    
    void ApplyItemEffects()
    {
        HungerSystem hungerSystem = FindObjectOfType<HungerSystem>();
        TemperatureSystem temperatureSystem = FindObjectOfType<TemperatureSystem>();
        
        if (hungerSystem != null && hungerRestored > 0)
        {
            hungerSystem.EatFood(hungerRestored);
            Debug.Log($"+{hungerRestored} Hunger");
        }
        
        if (temperatureSystem != null && temperatureRestored > 0)
        {
            temperatureSystem.IncreaseTemperature(temperatureRestored);
            Debug.Log($"+{temperatureRestored} Temperature");
        }
    }
}
