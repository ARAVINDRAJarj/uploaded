using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{
    [Header("Movement")]
    public float forwardSpeed = 15f;
    public float sidewaysSpeed = 12f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;

    [Header("UI & Game Over")]
    public GameObject gameOverPanel;              // Drag GameOverPanel here (in scene)
    public TextMeshProUGUI finalScoreText;        // Drag FinalScoreText (TMP) here
    public TextMeshProUGUI finalTimeText;         // Drag FinalTimeText (TMP) here
    public Button restartButton;                  // Drag Restart Button (optional)
    public Button mainMenuButton;                 // Drag Main Menu Button (optional)
    private int highScore = 0;
    public TextMeshProUGUI highScoreText;        // drag High Score UI here



    // gameplay state
    private Rigidbody rb;
    private bool isGameOver = false;
    private int coinCount = 0;
    private float elapsedTime = 0f;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timerText;
    private float survivalTime = 0f;


    [Header("Difficulty Settings")]
    public float difficultyIncreaseInterval = 30f; // every 10 seconds
    public float speedIncreaseAmount = 10f;         // increase by this much
    private float nextDifficultyTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        // Make sure GameOver panel is hidden at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Optional: hook up restart button if you want to wire via script
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        if (coinText != null)
            coinText.text = "Coins: 0";
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Load saved high score
        UpdateHighScoreUI();

        PlayerPrefs.DeleteKey("HighScore");


    }

    void Update()
    {
        if (!isGameOver)
        {
            // accumulate survival time
            elapsedTime += Time.deltaTime;
            // (optional) update a live timer UI here if you have one
        }

        // debug restart hotkey while testing
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.FloorToInt(survivalTime).ToString();
        }

        // Optional restart key for testing
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }

        if (!isGameOver && survivalTime >= nextDifficultyTime)
        {
            nextDifficultyTime += difficultyIncreaseInterval;
            IncreaseDifficulty();
        }

    }

    void UpdateHighScoreUI()
{
    if (highScoreText != null)
        highScoreText.text = "High Score: " + highScore.ToString();
}


    void FixedUpdate()
    {
        if (isGameOver) return;

        // Physics-based movement while keeping gravity intact
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.z = forwardSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        newVelocity.x = horizontal * sidewaysSpeed;
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);

        // clamp X position
        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        rb.MovePosition(pos);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Coin pickup
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);

            if (coinText != null)
                coinText.text = "Coins: " + coinCount;

            // ✅ High score check and save
            if (coinCount > highScore)
            {
                highScore = coinCount;
                PlayerPrefs.SetInt("HighScore", highScore);
                UpdateHighScoreUI();
            }

        }
        // Deadly collisions (snakes, obstacles)
        else if (other.CompareTag("Snake") || other.CompareTag("Obstacle"))
        {
            Debug.Log($"[DEBUG] Hit deadly object: {other.tag}. Triggering GameOver.");
            GameOver();
        }
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);

            if (coinText != null)
                coinText.text = "Coins: " + coinCount;
        }
        if (other.CompareTag("Coin"))
        {
            SoundManager.instance.PlayCoin();
            // existing coin logic
        }
        if (other.CompareTag("Snake") || other.CompareTag("Obstacle"))
        {
            SoundManager.instance.PlayHit();
            // existing game-over logic
        }
       

    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop gameplay
        rb.linearVelocity = Vector3.zero;
        // Pause time-based systems if desired (but UI will still work)
        Time.timeScale = 0f;

        // Debug print the values we will show
        Debug.Log($"[DEBUG] GameOver called - coins: {coinCount}, elapsedTime: {elapsedTime}");

        // Activate panel and set UI texts (null-checks for safety)
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        else Debug.LogWarning("GameOverPanel reference is null on PlayerController.");

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + coinCount.ToString();
        else Debug.LogWarning("finalScoreText not assigned in Inspector.");

        if (finalTimeText != null)
            finalTimeText.text = "Time: " + Mathf.FloorToInt(elapsedTime).ToString() + "s";
        else Debug.LogWarning("finalTimeText not assigned in Inspector.");

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + coinCount.ToString();

        if (finalTimeText != null)
            finalTimeText.text = "Time: " + Mathf.FloorToInt(elapsedTime).ToString() + "s";

        // ✅ Show High Score too (if you have a text in the GameOverPanel)
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore.ToString();



    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // restore time scale before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScenne");
    }

    void IncreaseDifficulty()
    {
        forwardSpeed += speedIncreaseAmount;
        sidewaysSpeed += 0.2f;

        // Update all obstacle spawners using the new Unity 6 API
        ObstacleSpawner[] spawners = FindObjectsByType<ObstacleSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            spawner.spawnInterval = Mathf.Max(0.5f, spawner.spawnInterval - 0.3f);
        }

        Debug.Log("Difficulty increased! New speed: " + forwardSpeed);
    }


}
