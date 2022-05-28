using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleIntro : MonoBehaviour
{
    //The code for the fading out the intro screen at the start of the battle

    public float timer;
    public Image image;
    public float timeSartFade;
    public float timeEndFade;
    public int mainMenu;
    public GameObject menu;
    public int menuActive;

    void Start()
    {
        timer = 0;
        menuActive = 0;
    }

    void FixedUpdate()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (0.1f * Time.deltaTime));
        timer += Time.deltaTime;

        if (timer >= timeSartFade)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (0.5f * Time.deltaTime));
        }

        if (timer >= timeEndFade)
        {         
            if (mainMenu == 1)
            {
                menu.SetActive(true);
                menuActive = 1;
            }

            gameObject.SetActive(false);
        }
    }
}
