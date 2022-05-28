using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPhaseIn : MonoBehaviour
{
    public TextMeshProUGUI image;

    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (0.2f * Time.deltaTime));
    }
}
