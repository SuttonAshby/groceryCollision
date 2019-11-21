using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool stages = false;
    public CanvasGroup carts;
    public CanvasGroup qrs;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Next() {
        if(stages) {
            StartGame();
            return;
        }
        ShowQRs();
        stages = true;
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("Aisle");
        _audioSource.Play();
    }

    public void ShowQRs() {
        qrs.alpha = 1f;
        carts.alpha = 0f;
    }

}
