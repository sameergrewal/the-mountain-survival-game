using UnityEngine;

public class SummitTrigger : MonoBehaviour
{
    public GameObject victoryPanel;
    
    private bool hasWon = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;
        
        if (other.CompareTag("Player"))
        {
            hasWon = true;
            TriggerVictory();
        }
    }
    
    void TriggerVictory()
    {
        Debug.Log("Player reached summit!");
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0f;
    }
}
