using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseCanvas : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public Text player1Text;
    public Text player2Text;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        hide();
    }

    void hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
    }

    void show() {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }

    public void Player1Win()
    {
        player1Text.text = "WIN";
        player2Text.text = "LOSE";
        show();
    }

    public void Player2Win() {
        player1Text.text = "LOSE";
        player2Text.text = "WIN";
        show();
    }

    public void RestartGame() {
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) { Player1Win(); }
    }
}
