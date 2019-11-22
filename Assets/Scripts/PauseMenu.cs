using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject pauseButtonObj;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseButtonObj.SetActive(false);
        Time.timeScale = 0f;
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        pauseButtonObj.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}
