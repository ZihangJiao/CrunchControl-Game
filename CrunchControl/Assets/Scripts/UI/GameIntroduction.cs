using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameIntroduction : MonoBehaviour
{
    public float timer;
    public GameObject continueText;
    public GameObject storyText01;
    public GameObject storyText02;
    public GameObject storyText03;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            storyText01.SetActive(true);
        }

        if (timer >= 16)
        {
            storyText02.SetActive(true);
        }

        if (timer >= 30)
        {
            storyText03.SetActive(true);
        }

        if (timer >= 42)
        {
            continueText.SetActive(true);
        }

        if (timer >= 144)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
