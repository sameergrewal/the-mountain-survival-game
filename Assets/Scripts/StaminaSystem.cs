using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    public float jumpStaminaCost = 15f;
    
    [Header("UI References")]
    public Image staminaBarFill;
    public Color fullColor = Color.yellow;
    public Color emptyColor = Color.red;
    
    private bool isExhausted = false;
    
    void Start()
    {
        currentStamina = maxStamina;
    }
    
    void Update()
    {
        UpdateStaminaUI();
    }
    
    public bool CanSprint()
    {
        return currentStamina > 0 && !isExhausted;
    }
    
    public bool CanJump()
    {
        return currentStamina >= jumpStaminaCost;
    }
    
    public void UseSprint(float deltaTime)
    {
        currentStamina -= staminaDrainRate * deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        
        if (currentStamina <= 0)
        {
            isExhausted = true;
        }
    }
    
    public void UseJump()
    {
        currentStamina -= jumpStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
    
    public void RegenerateStamina(float deltaTime)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            
            if (currentStamina >= maxStamina * 0.3f)
            {
                isExhausted = false;
            }
        }
    }
    
    void UpdateStaminaUI()
    {
        if (staminaBarFill != null)
        {
            float fillPercent = currentStamina / maxStamina;
            staminaBarFill.fillAmount = fillPercent;
            
            staminaBarFill.color = Color.Lerp(emptyColor, fullColor, fillPercent);
        }
    }
}
