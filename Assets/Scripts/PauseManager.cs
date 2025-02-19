using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseBackground;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;   // Кнопка паузы
    [SerializeField] private GameObject CellContainer;
    [SerializeField] private GameTimer gameTimer;

    private bool isPaused = false; // Флаг паузы

    private void Start()
    {
        // Назначаем обработчики событий
        pauseButton.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Переключаем состояние паузы

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Останавливаем время в игре
        pauseBackground.SetActive(true);
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false); // Скрываем кнопку паузы
        CellContainer.SetActive(false);

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Возобновляем время в игре
        pauseBackground.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true); // Показываем кнопку паузы
        CellContainer.SetActive(true);

        if (gameTimer != null)
        {
            gameTimer.ResumeTimer();
        }
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Возобновляем время в игре
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene"); // Загружаем главное меню
    }
}