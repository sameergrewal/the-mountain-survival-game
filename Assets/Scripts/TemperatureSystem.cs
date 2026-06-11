using UnityEngine;
using UnityEngine.UI;

public class TemperatureSystem : MonoBehaviour
{
    [Header("Temperature Settings")]
    public float maxTemperature = 100f;
    public float currentTemperature;
    
    [Header("Altitude Settings")]
    public float safeAltitude = 50f;
    public float dangerAltitude = 200f;
    public float maxTempLossRate = 10f;
    
    [Header("Player Reference")]
    public Transform player;
    
    [Header("UI References")]
    public Image temperatureBarFill;
    public Color warmColor = Color.cyan;
    public Color coldColor = Color.blue;
    
    [Header("Game Over")]
    public bool isDead = false;
    
    void Start()
    {
        currentTemperature = maxTemperature;
        
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }
    
    void Update()
    {
        if (isDead || player == null) return;
        
        float currentAltitude = player.position.y;
        
        float temperatureLoss = CalculateTemperatureLoss(currentAltitude);
        
        currentTemperature -= temperatureLoss * Time.deltaTime;
        currentTemperature = Mathf.Clamp(currentTemperature, 0f, maxTemperature);
        
        UpdateTemperatureUI();
        
        if (currentTemperature <= 0)
        {
            Die();
        }
    }
    
    float CalculateTemperatureLoss(float altitude)
    {
        if (altitude <= safeAltitude)
        {
            return 0f;
        }
        
        if (altitude >= dangerAltitude)
        {
            return maxTempLossRate;
        }
        
        float altitudeRange = dangerAltitude - safeAltitude;
        float altitudeProgress = (altitude - safeAltitude) / altitudeRange;
        
        return maxTempLossRate * altitudeProgress;
    }
    
    void UpdateTemperatureUI()
    {
        if (temperatureBarFill != null)
        {
            float fillPercent = currentTemperature / maxTemperature;
            temperatureBarFill.fillAmount = fillPercent;
            
            temperatureBarFill.color = Color.Lerp(coldColor, warmColor, fillPercent);
        }
    }
    
    public void IncreaseTemperature(float amount)
    {
        currentTemperature += amount;
        currentTemperature = Mathf.Clamp(currentTemperature, 0f, maxTemperature);
    }
    
    void Die()
    {
        isDead = true;
        GameManager.TriggerGameOver("YOU FROZE TO DEATH");
    }
}
