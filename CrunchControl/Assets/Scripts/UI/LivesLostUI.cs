using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesLostUI : MonoBehaviour
{
    //This code makes it so that when set to active, the Screen deactives itself a few seconds later

    public float timer;

    void Start()
    {
        timer = 0;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 3)
        {
            gameObject.SetActive(false);
        }
    }
}
