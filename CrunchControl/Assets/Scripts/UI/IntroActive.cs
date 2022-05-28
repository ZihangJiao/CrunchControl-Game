using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroActive : MonoBehaviour
{
    //This code is used to show the intro screen at the start of the battle even if it's off in the editor
    public GameObject intro;

    void Start()
    {
        intro.SetActive(true);
    }
}
