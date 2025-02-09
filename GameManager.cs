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
    //if rigging is on, aiIntention: 0 => tie, 1 => win, 2 => throw
    private int[] intentionArray = {0,0,0,0,0,0,0,0,0,0};
    //stores all intentions for the 10 game round based on presets
    private ROUND_NUMBER; //THERE IS PROBABLY ALREADY A WAY OF RECORDING THIS
    //so delete this. it is called in getAIChoice

    void Start()
    {
        Debug.Log("GameManager Initialized!");
        resultText.text = "";

        // Add listeners to the buttons
        rockButton.onClick.AddListener(() => PlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => PlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => PlayerChoice(Choice.Scissors));

        int gameSeed = Random.Range(0,100);

        if(gameSeed < 67)
        {
                activeRigging = true;
            if(gameSeed < 1)
                intentionArray = {1,1,1,1,1,1,1,1,1,1}; //'truthnuke'
            else if(gameSeed < 11)
                intentionArray = {2,1,2,1,2,1,2,1,0,1}; //'to-and-fro'
            else if(gameSeed < 21)
                intentionArray = {2,1,1,0,0,0,0,0,0,0}; //'no comment'
            else if(gameSeed < 31)
                intentionArray = {2,2,1,0,1,0,0,1,2,1}; //'modified to and fro'
            else if(gameSeed < 41)
                intentionArray = {0,2,0,2,1,2,1,1,0,0}; //'force tie'
            else if(gameSeed < 51)
                intentionArray = {0,1,1,1,1,1,2,2,2,2}; //'forfeit'
            else if(gameSeed < 61)
                intentionArray = {1,0,0,2,0,0,0,0,0,1} //''
            else if(gameSeed < 63)
                intentionArray = {0,0,0,0,0,0,0,0,0,1} //'friend_maker'
            else if(gameSeed < 65)
                intentionArray = {0,0,0,0,0,0,0,0,0,0} //'FORCE_TIE'
            else if(gameSeed < 67)
                intentionArray = {2,2,2,2,2,2,2,2,2,2} //'report player'
        }
        
        //so overall:
        //a current 13% chance for player win
        //a current 23% chance for tie
        //a current 64% chance for ai win
        
    }

    // This method will be triggered when a player clicks on a choice (Rock, Paper, or Scissors)
    public void PlayerChoice(Choice playerChoice)
    {
        // Handle the player's choice
        string choiceName = playerChoice.ToString();  // Convert enum to string (e.g., Rock, Paper, Scissors)
        HandleObjectClick(choiceName);  // Call the existing object-click handler with this choice
        
        Debug.Log("Player chose " + choiceName);
        // Generate AI's choice
        Choice aiChoice = GetAIChoice(playerChoice);
        
        Debug.Log("AI chose " + aiChoice.ToString());
        // Determine the result of the game
        string result = DetermineWinner(playerChoice, aiChoice);
        
        // Display the result
        //resultText.text = result;
        HandleResult(result);

    }

    private Choice CounterPick(Choice otherChoice, int intention)
    {
        //apologies for making an int keyword, but that is easiest
        //intention should be set to 0 for "tie", 1 for "win", 2 for "throw"
        
        //note that for Choice, 0 casts to Rock, 1 to paper, 2 to sciss
        Choice bucket;
        if(otherChoice==Choice.Rock)
        {
            bucket = (Choice)((0 + intention) % 3);
        }
        else if(otherChoice==Choice.Paper)
        {
            bucket = (Choice)((1 + intention) % 3);
        }
        else //guaranteed to be Scissors
        {
            bucket = (Choice)((2 + intention) % 3);
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
        Choice aiChoice = GetAIChoice(playerChoice);
        string result = DetermineWinner(playerChoice, aiChoice);
        
        // Display the result
        HandleResult(result);
        resultText.text = result;
    }

    // Randomly determines the AI's choice
    Choice GetAIChoice(Choice playerChoice)
    {
        Choice aiChoice;
        if(activeRigging)
        {
            aiChoice = CounterPick(playerChoice, intentionArray[ROUND_NUMBER]);
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
