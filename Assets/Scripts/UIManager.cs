using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI timeOfDayText;
    
    [Header("Player Reference")]
    public Transform player;
    
    [Header("Settings")]
    public string altitudeUnit = "m";
    
    private DayNightCycle dayNightCycle;
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }
    
    void Update()
    {
        UpdateAltitude();
        UpdateTimeOfDay();
    }
    
    void UpdateAltitude()
    {
        if (player == null || altitudeText == null) return;
        
        float altitude = player.position.y;
        int altitudeRounded = Mathf.RoundToInt(altitude);
        
        altitudeText.text = $"Altitude: {altitudeRounded}{altitudeUnit}";
    }
    
    void UpdateTimeOfDay()
    {
        if (dayNightCycle != null && timeOfDayText != null)
        {
            string timeString = dayNightCycle.GetTimeOfDayString();
            timeOfDayText.text = timeString;
        }
    }
}
