using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [Header("Weather Particles")]
    public ParticleSystem snowParticles;
    public ParticleSystem windParticles;
    
    [Header("Altitude Settings")]
    public float snowStartAltitude = 200f;
    public float maxSnowAltitude = 400f;
    
    [Header("Player Reference")]
    public Transform player;
    
    [Header("Temperature Effect")]
    public TemperatureSystem temperatureSystem;
    public float snowTempModifier = 1.5f;
    
    private float baseMaxTempLoss;
    private bool isInSnow = false;
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        if (temperatureSystem == null)
        {
            temperatureSystem = FindObjectOfType<TemperatureSystem>();
        }
        
        if (temperatureSystem != null)
        {
            baseMaxTempLoss = temperatureSystem.maxTempLossRate;
        }
        
        if (snowParticles != null)
        {
            snowParticles.Stop();
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        UpdateWeatherBasedOnAltitude();
        UpdateTemperatureEffect();
    }
    
    void UpdateWeatherBasedOnAltitude()
    {
        float altitude = player.position.y;
        
        if (altitude >= snowStartAltitude)
        {
            if (!isInSnow)
            {
                StartSnow();
            }
            
            float snowIntensity = Mathf.Clamp01((altitude - snowStartAltitude) / (maxSnowAltitude - snowStartAltitude));
            
            if (snowParticles != null)
            {
                var emission = snowParticles.emission;
                emission.rateOverTime = Mathf.Lerp(20f, 100f, snowIntensity);
                
                Vector3 followPosition = player.position;
                followPosition.y = player.position.y + 50f;
                snowParticles.transform.position = followPosition;
            }
        }
        else
        {
            if (isInSnow)
            {
                StopSnow();
            }
        }
    }
    
    void StartSnow()
    {
        isInSnow = true;
        
        if (snowParticles != null)
        {
            snowParticles.Play();
        }
        
        if (windParticles != null)
        {
            windParticles.Play();
        }
        
        Debug.Log("Entered snow zone");
    }
    
    void StopSnow()
    {
        isInSnow = false;
        
        if (snowParticles != null)
        {
            snowParticles.Stop();
        }
        
        if (windParticles != null)
        {
            windParticles.Stop();
        }
        
        Debug.Log("Left snow zone");
    }
    
    void UpdateTemperatureEffect()
    {
        if (temperatureSystem == null) return;
        
        if (isInSnow)
        {
            temperatureSystem.maxTempLossRate = baseMaxTempLoss * snowTempModifier;
        }
        else
        {
            temperatureSystem.maxTempLossRate = baseMaxTempLoss;
        }
    }
}
