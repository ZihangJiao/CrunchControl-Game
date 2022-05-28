using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCardImage : MonoBehaviour
{
    //This code is used to control the random card on the main menu

    public Sprite[] cardImage;
    public Image display;
    public GameObject[] targets;
    public float timer;

    void Start()
    {
        display.sprite = cardImage[Random.Range(0, 19)];
        timer = 0;
        gameObject.transform.position = targets[Random.Range(0, 5)].transform.position;
    }

    void FixedUpdate()
    {
        if (timer <= 6)
        {
            timer += Time.deltaTime;

            if (timer <= 3)
            {
                display.color = new Color(display.color.r, display.color.g, display.color.b, display.color.a + (0.5f * Time.deltaTime));
            }

            if (timer > 3)
            {
                display.color = new Color(display.color.r, display.color.g, display.color.b, display.color.a - (0.5f * Time.deltaTime));
            }
        }
        else
        {
            display.sprite = cardImage[Random.Range(0, 19)];
            timer = 0;
            gameObject.transform.position = targets[Random.Range(0, 5)].transform.position;
        }
    }
}
