using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    public Image image;

    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (0.25f * Time.deltaTime));
    }
}
