using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Skybox")]
    public Material blendingSkybox;
    public string skyboxBlendProperty = "_Blend";
    
    [Header("Sun")]
    public Light sunLight;
    
    [Header("Time Settings")]
    public float dayLengthInSeconds = 120f;
    public float currentTimeOfDay = 0.3f;
    
    [Header("Temperature")]
    public TemperatureSystem temperatureSystem;
    
    private float timeSpeed;
    
    void Start()
    {
        timeSpeed = 1f / dayLengthInSeconds;
        
        if (temperatureSystem == null)
        {
            temperatureSystem = FindObjectOfType<TemperatureSystem>();
        }
        
        if (blendingSkybox != null)
        {
            RenderSettings.skybox = blendingSkybox;
        }
    }
    
    void Update()
    {
        currentTimeOfDay += timeSpeed * Time.deltaTime;
        
        if (currentTimeOfDay >= 1f)
        {
            currentTimeOfDay = 0f;
        }
        
        UpdateSkyboxBlend();
        UpdateSun();
        UpdateTemperature();
    }
    
    void UpdateSkyboxBlend()
    {
        if (blendingSkybox == null) return;
        
        float blendValue = CalculateSkyboxBlend();
        
        if (blendingSkybox.HasProperty(skyboxBlendProperty))
        {
            blendingSkybox.SetFloat(skyboxBlendProperty, blendValue);
        }
    }
    
    float CalculateSkyboxBlend()
    {
        if (currentTimeOfDay < 0.25f)
        {
            return 1f;
        }
        else if (currentTimeOfDay < 0.5f)
        {
            return Mathf.Lerp(1f, 0f, (currentTimeOfDay - 0.25f) / 0.25f);
        }
        else if (currentTimeOfDay < 0.75f)
        {
            return Mathf.Lerp(0f, 1f, (currentTimeOfDay - 0.5f) / 0.25f);
        }
        else
        {
            return 1f;
        }
    }
    
    void UpdateSun()
    {
        if (sunLight == null) return;
        
        float sunAngle = currentTimeOfDay * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);
        
        float intensity = CalculateSunIntensity();
        sunLight.intensity = intensity;
        
        Color sunColor = CalculateSunColor();
        sunLight.color = sunColor;
        RenderSettings.ambientLight = sunColor * 0.5f;
    }
    
    float CalculateSunIntensity()
    {
        if (currentTimeOfDay < 0.25f) return 0f;
        if (currentTimeOfDay < 0.3f) return Mathf.Lerp(0f, 1f, (currentTimeOfDay - 0.25f) / 0.05f);
        if (currentTimeOfDay < 0.7f) return 1f;
        if (currentTimeOfDay < 0.75f) return Mathf.Lerp(1f, 0f, (currentTimeOfDay - 0.7f) / 0.05f);
        return 0f;
    }
    
    Color CalculateSunColor()
    {
        if (currentTimeOfDay < 0.25f) return new Color(0.1f, 0.1f, 0.2f);
        if (currentTimeOfDay < 0.3f) return Color.Lerp(new Color(0.1f, 0.1f, 0.2f), new Color(1f, 0.8f, 0.6f), (currentTimeOfDay - 0.25f) / 0.05f);
        if (currentTimeOfDay < 0.5f) return Color.white;
        if (currentTimeOfDay < 0.7f) return Color.white;
        if (currentTimeOfDay < 0.75f) return Color.Lerp(Color.white, new Color(1f, 0.5f, 0.3f), (currentTimeOfDay - 0.7f) / 0.05f);
        return new Color(0.1f, 0.1f, 0.2f);
    }
    
    void UpdateTemperature()
    {
        if (temperatureSystem == null) return;
        
        bool isNight = currentTimeOfDay > 0.75f || currentTimeOfDay < 0.25f;
        
        if (isNight)
        {
            temperatureSystem.maxTempLossRate = 15f;
        }
        else
        {
            temperatureSystem.maxTempLossRate = 10f;
        }
    }
    
    public string GetTimeOfDayString()
    {
        if (currentTimeOfDay < 0.25f) return "Night";
        if (currentTimeOfDay < 0.5f) return "Morning";
        if (currentTimeOfDay < 0.75f) return "Afternoon";
        return "Evening";
    }
}
