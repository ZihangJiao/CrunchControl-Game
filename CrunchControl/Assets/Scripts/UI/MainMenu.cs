using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //This script manages the main menu

    public GameObject mainMenuUI;
    public GameObject options;
    public GameObject help;
    public GameObject cardLibrary;
    public GameObject audio;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadOptions()
    {
        options.SetActive(true);
        mainMenuUI.SetActive(false);
        help.SetActive(false);
        cardLibrary.SetActive(false);
    }

    public void LoadHelp()
    {
        help.SetActive(true);
        mainMenuUI.SetActive(false);
        options.SetActive(false);
        cardLibrary.SetActive(false);
    }

    public void LoadCardLibrary()
    {
        cardLibrary.SetActive(true);
        mainMenuUI.SetActive(false);
        options.SetActive(false);
        help.SetActive(false);
    }

    public void LoadMainMenu()
    {
        mainMenuUI.SetActive(true);
        options.SetActive(false);
        help.SetActive(false);
        cardLibrary.SetActive(false);
        audio.SetActive(true);
    }
}
