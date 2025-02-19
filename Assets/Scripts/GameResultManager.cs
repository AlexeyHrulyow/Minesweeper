using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
    [SerializeField] GameObject gameResultPanel;
    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] private GameObject CellContainer;
    [SerializeField] private GameTimer gameTimer;

    public void ShowGameResult(string message, int difficulty)
    {
        if (gameResultPanel != null)
        {
            messageText.text = message;
            gameResultPanel.SetActive(true);
            CellContainer.SetActive(false);

            if (message.Contains("выиграли"))
            {
                float time = gameTimer.GetElapsedTime();
                RecordManager.SaveRecord(difficulty, time);
            }
        }

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
    }

    public void ReturnToMainMenu()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)   
        {
            inputManager.SetMineField(null);
        }

        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);

        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ResetMainMenu();
        }
    }
}