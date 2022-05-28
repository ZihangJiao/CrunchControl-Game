using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    //This script manages the help menu

    public GameObject helpMenuUI;
    public GameObject objectives;
    public GameObject controls;
    public GameObject credits;

    public void LoadHelpMenu()
    {
        helpMenuUI.SetActive(true);
        objectives.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(false);
    }

    public void LoadObjectives()
    {
        helpMenuUI.SetActive(false);
        objectives.SetActive(true);
        controls.SetActive(false);
        credits.SetActive(false);
    }

    public void LoadControls()
    {
        helpMenuUI.SetActive(false);
        objectives.SetActive(false);
        controls.SetActive(true);
        credits.SetActive(false);
    }

    public void LoadCredits()
    {
        helpMenuUI.SetActive(false);
        objectives.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(true);
    }
}
