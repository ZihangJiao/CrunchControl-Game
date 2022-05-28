using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePauseMenu : MonoBehaviour
{
    //This script manages the pause menu

    public GameObject pauseMenuUI;
    public GameObject options;
    public GameObject help;
    public GameObject cardLibrary;
    public int isBattle; // 0=no, 1=yes

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        options.SetActive(false);
        help.SetActive(false);
        cardLibrary.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        options.SetActive(false);
        help.SetActive(false);
        cardLibrary.SetActive(false);

    }

    public void LoadMainMenu()
    {
        if (isBattle == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (isBattle == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }
    }

    public void LoadOptions()
    {
        pauseMenuUI.SetActive(false);
        options.SetActive(true);
        help.SetActive(false);
        cardLibrary.SetActive(false);
    }

    public void LoadHelp()
    {
        pauseMenuUI.SetActive(false);
        options.SetActive(false);
        help.SetActive(true);
        cardLibrary.SetActive(false);
    }

    public void LoadCardLibrary()
    {
        pauseMenuUI.SetActive(false);
        options.SetActive(false);
        help.SetActive(false);
        cardLibrary.SetActive(true);
    }

    public void PauseTopDown()
    {
        Time.timeScale = 0f;
    }

    public void ResumeTopDown()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();

            if (isBattle == 0)
            {
                PauseTopDown();
            }
        }
    }
}
