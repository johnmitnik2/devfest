using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour {

void Start() {
    Debug.Log("UI Manager is ethereal");
}

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private TextMeshProUGUI currentResultText;

    public void UpdateScore(int playerScore, int aiScore)
    {
        playerScoreText.text = $"Player: {playerScore}";
        aiScoreText.text = $"AI: {aiScore}";
    }
}
