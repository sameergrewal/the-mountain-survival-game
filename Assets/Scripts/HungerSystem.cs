using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float hungerDecayRate = 5f;
    public float currentHunger;
    
    [Header("UI References")]
    public Image hungerBarFill;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;
    
    [Header("Game Over")]
    public bool isDead = false;
    
    void Start()
    {
        currentHunger = maxHunger;
    }
    
    void Update()
    {
        if (!isDead)
        {
            currentHunger -= hungerDecayRate * Time.deltaTime / 60f;
            currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
            
            UpdateHungerUI();
            
            if (currentHunger <= 0)
            {
                Die();
            }
        }
    }
    
    void UpdateHungerUI()
    {
        if (hungerBarFill != null)
        {
            float fillPercent = currentHunger / maxHunger;
            hungerBarFill.fillAmount = fillPercent;
            
            hungerBarFill.color = Color.Lerp(emptyColor, fullColor, fillPercent);
        }
    }
    
    public void EatFood(float foodValue)
    {
        currentHunger += foodValue;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
    }
    
    void Die()
    {
        isDead = true;
        GameManager.TriggerGameOver("YOU STARVED TO DEATH");
    }
}
