using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance { get; private set; }

    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] private TMP_Text easyRecordText;
    [SerializeField] private TMP_Text mediumRecordText;
    [SerializeField] private TMP_Text hardRecordText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetMainMenu();
        UpdateRecords();
    }

    private void UpdateRecords()
    {
        easyRecordText.text = FormatTime(RecordManager.GetRecord(5));
        mediumRecordText.text = FormatTime(RecordManager.GetRecord(10));
        hardRecordText.text = FormatTime(RecordManager.GetRecord(15));
    }

    private string FormatTime(float time)
    {
        if (time == float.MaxValue) return "Нет рекорда";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void ResetMainMenu()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }

        if (playButton != null && exitButton != null)
        {
            playButton.SetActive(true);
            exitButton.SetActive(true);
        }

        var playButtonComponent = playButton.GetComponent<UnityEngine.UI.Button>();
        var exitButtonComponent = exitButton.GetComponent<UnityEngine.UI.Button>();

        if (playButtonComponent != null) playButtonComponent.interactable = true;
        if (exitButtonComponent != null) exitButtonComponent.interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
        }

        if (playButton != null && exitButton != null)
        {
            playButton.SetActive(false);
            exitButton.SetActive(false);
        }
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnDifficultySelected(int width, int height)
    {
        GameData.FieldWidth = width;
        GameData.FieldHeight = height;
        HideMainMenu();
        SceneManager.LoadScene("GameScene");
    }

    public void OnEasyButtonClicked()
    {
        OnDifficultySelected(5, 5);
    }

    public void OnMediumButtonClicked()
    {
        OnDifficultySelected(10, 10);
    }

    public void OnHardButtonClicked()
    {
        OnDifficultySelected(15, 15);
    }

    public void OnBackButtonClicked()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }

        if (playButton != null && exitButton != null)
        {
            playButton.SetActive(true);
            exitButton.SetActive(true);
        }
    }

    private void HideMainMenu()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ResetMainMenu();

        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.EnableInput(true);
        }
    }
}