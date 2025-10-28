using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;
    public Button restartButton;
    public Button mainMenuButton;

    private float elapsedTime;
    private bool isGameOver = false;
    private int coinCount = 0;

  
    void Update()
    {
        if (!isGameOver)
            elapsedTime += Time.deltaTime;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + coinCount;

        if (finalTimeText != null)
            finalTimeText.text = "Time: " + Mathf.FloorToInt(elapsedTime) + "s";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // we'll set this up in Step 8.3
    }
}
