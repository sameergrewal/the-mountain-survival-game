using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI deathCauseText;
    public CanvasGroup gameOverCanvasGroup;
    
    [Header("Fade Settings")]
    public float fadeDuration = 2f;
    
    [Header("Game State")]
    private bool isGameOver = false;
    
    private static GameManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        Time.timeScale = 1f;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void GameOver(string causeOfDeath)
    {
        if (isGameOver) return;
        
        isGameOver = true;
        
        StartCoroutine(FadeToGameOver(causeOfDeath));
    }
    
    IEnumerator FadeToGameOver(string causeOfDeath)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
        }
        
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            
            if (gameOverCanvasGroup != null)
            {
                gameOverCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            }
            
            yield return null;
        }
        
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 1f;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        
        if (deathCauseText != null)
        {
            deathCauseText.text = causeOfDeath;
        }
        
        if (gameOverText != null)
        {
            gameOverText.text = "YOU DIED";
        }
        
        Debug.Log($"Game Over: {causeOfDeath}");
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    public static void TriggerGameOver(string cause)
    {
        if (instance != null)
        {
            instance.GameOver(cause);
        }
    }
}
