using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenuUI;

    public void PlayGame()
    {
        startMenuUI.SetActive(false);
        Time.timeScale = 1f; // resume game
    }

    void Start()
    {
        startMenuUI.SetActive(true);
        Time.timeScale = 0f; // pause until player clicks Play
    }
}
