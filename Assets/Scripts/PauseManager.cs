using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseBackground;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;   // ������ �����
    [SerializeField] private GameObject CellContainer;
    [SerializeField] private GameTimer gameTimer;

    private bool isPaused = false; // ���� �����

    private void Start()
    {
        // ��������� ����������� �������
        pauseButton.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // ����������� ��������� �����

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
        Time.timeScale = 0f; // ������������� ����� � ����
        pauseBackground.SetActive(true);
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false); // �������� ������ �����
        CellContainer.SetActive(false);

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // ������������ ����� � ����
        pauseBackground.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true); // ���������� ������ �����
        CellContainer.SetActive(true);

        if (gameTimer != null)
        {
            gameTimer.ResumeTimer();
        }
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // ������������ ����� � ����
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene"); // ��������� ������� ����
    }
}