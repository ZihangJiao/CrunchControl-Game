using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public float fadeSpeed;
    public int fadeDir = -1;// -1 means becoming dark, 1 means becoming bright

    void Start()
    {
        Image img = GetComponent<Image>();
        Color color = img.color;
        color.a = 0.5f;
        img.color = color;
       
    }

    void Update()
    {
        Image img = GetComponent<Image>();
        Color color = img.color;
        color.a += fadeSpeed * Time.deltaTime;
        Debug.Log(fadeSpeed);
        //Debug.Log(Time.deltaTime);
        img.color = color;
    }

    public float BeginChangeScene(int direction)
    {
        fadeDir = direction;
        return 1 / fadeSpeed;
    }

    void OnSceneLoaded()
    {
        Debug.Log("Loaded");
        BeginChangeScene(-1);
    }

}
