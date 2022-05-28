using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantumSpinner : MonoBehaviour
{
    public Image image;
    public float rotSpeed;

    private void Start()
    {
        rotSpeed = 0;
    }

    void Update()
    {
        image.color = new Color(image.color.r - (0.2f * Time.deltaTime), image.color.g - (0.2f * Time.deltaTime), image.color.b, image.color.a + (0.4f * Time.deltaTime));

        rotSpeed += ((Time.deltaTime / 9f)+ (rotSpeed / 90f));

        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}
