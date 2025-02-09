using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public enum Choice { Rock, Paper, Scissors }

    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button rockButton, paperButton, scissorsButton;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    //[SerializeField] private UIManager ui;

    private int playerScore = 0;
    private int aiScore = 0;
    private bool activeRigging = false;

    void Start()
    {
        Debug.Log("GameManager Initialized!");
        resultText.text = "";

        // Add listeners to the buttons
        rockButton.onClick.AddListener(() => PlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => PlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => PlayerChoice(Choice.Scissors));
    }

    // This method will be triggered when a player clicks on a choice (Rock, Paper, or Scissors)
    public void PlayerChoice(Choice playerChoice)
    {
        // Handle the player's choice
        string choiceName = playerChoice.ToString();  // Convert enum to string (e.g., Rock, Paper, Scissors)
        HandleObjectClick(choiceName);  // Call the existing object-click handler with this choice
        
        Debug.Log("Player chose " + choiceName);
        // Generate AI's choice
        Choice aiChoice = GetAIChoice(playerChoice, activeRigging);
        
        Debug.Log("AI chose " + aiChoice.ToString());
        // Determine the result of the game
        string result = DetermineWinner(playerChoice, aiChoice);
        
        // Display the result
        //resultText.text = result;
        HandleResult(result);

    }

    private Choice CounterPick(Choice otherChoice)
    {
        Choice bucket;
        if(otherChoice==Choice.Rock)
        {
            bucket = Choice.Paper;
        }
        else if(otherChoice==Choice.Paper)
        {
            bucket==Choice.Scissors;
        }
        else //guaranteed to be Scissors
        {
            bucket==Choice.Rock;
        }
        return bucket;
    }

    // This method handles the outcome of the player's choice
    public void HandleObjectClick(string objectName)
    {
        // You can expand the logic for specific object interactions here
        switch (objectName)
        {
            case "Rock":
                Debug.Log("Player chose Rock!");
                break;
            case "Paper":
                Debug.Log("Player chose Paper!");
                break;
            case "Scissors":
                Debug.Log("Player chose Scissors!");
                break;
            default:
                Debug.Log("Unknown choice!");
                break;
        }
        Choice playerChoice = (Choice)Enum.Parse(typeof(Choice), objectName);
        Choice aiChoice = GetAIChoice(playerChoice, activeRigging);
        string result = DetermineWinner(playerChoice, aiChoice);
        
        // Display the result
        HandleResult(result);
        resultText.text = result;
    }

    // Randomly determines the AI's choice
    Choice GetAIChoice(Choice playerChoice, bool blatantRigging)
    {
        Choice aiChoice;
        if(blatantRigging)
        {
            aiChoice = CounterPick(playerChoice);
        }
        else
        {
            Choice aiChoice = (Choice)Random.Range(0, 3); // Random AI choice
        }
        return aiChoice;
    }

    // Determines the winner of the game
    string DetermineWinner(Choice playerChoice, Choice aiChoice)
    {
        if (playerChoice == aiChoice)
            return "It's a tie!";
        
        if ((playerChoice == Choice.Rock && aiChoice == Choice.Scissors) ||
            (playerChoice == Choice.Paper && aiChoice == Choice.Rock) ||
            (playerChoice == Choice.Scissors && aiChoice == Choice.Paper))
        {
            playerScore++;
            UpdateScore();  // Update score UI
            return "You Win!";
        }
        else
        {
            aiScore++;
            UpdateScore();  // Update score UI
            return "AI Wins!";
        }
    }

    public void HandleResult(string result)
    {
        // Show the result text
        resultText.alpha = 1;
        resultText.text = result;
        
        // Start the coroutine to hide the text after a few seconds
        StartCoroutine(FadeOutResultText(0.9f, 0.2f));  // 3 seconds delay
    }

    private IEnumerator FadeOutResultText(float delay, float fadeDuration)
{
    // Wait for the initial delay
    yield return new WaitForSeconds(delay);
    
    float startAlpha = resultText.alpha;
    
    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    {
        // Gradually reduce the alpha to 0 (fade out)
        float normalizedTime = t / fadeDuration;
        resultText.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime);
        yield return null;
    }
    
    // After fade, set alpha to 0 and clear text
    resultText.alpha = 0;
    resultText.text = "";
}
    public void UpdateScore() //this removes the need for UIManager
    {
        playerScoreText.text = $"Player: {playerScore}";
        aiScoreText.text = $"AI: {aiScore}";
    }


}
