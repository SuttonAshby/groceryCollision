using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioSource _audioSource;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Aisle", LoadSceneMode.Single);
        _audioSource.Play();
    }
}
