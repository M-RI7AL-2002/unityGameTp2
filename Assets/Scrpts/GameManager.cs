using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public Text instructionsText;
    public Text scoreText;
    public Text feedbackText;
    public Text timerText;

    [Header("Game Settings")]
    public float gameDuration = 60f; // 60 seconds timer

    private int score = 0;
    private float remainingTime;
    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        remainingTime = gameDuration;

        scoreText.text = "Score: 0";
        feedbackText.text = "";
     
        UpdateTimerUI();
    }

    void Update()
    {
        if (gameOver) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            GameOver();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    private void GameOver()
    {
        gameOver = true;
        feedbackText.text = "Time's up!";
        instructionsText.text = "Game Over!";
        Debug.Log("Game Over!");
    }

    public bool CheckTrashDrop(TrashItem trash, Bin bin)
    {
        if (gameOver) return false;

        if (trash.trashType == bin.binType)
        {
            score++;
            feedbackText.text = $"Correct! {trash.trashType} → {bin.binType}";
            instructionsText.text = "Keep sorting!";
            Debug.Log($"Correct: {trash.name} → {bin.name} at {Time.time}");

            Destroy(trash.gameObject);
            scoreText.text = "Score: " + score;
            return true;
        }
        else
        {
            feedbackText.text = $"Wrong bin! Try again.";
            Debug.Log($"Wrong: {trash.name} → {bin.name} at {Time.time}");
            return false;
        }
    }
}
