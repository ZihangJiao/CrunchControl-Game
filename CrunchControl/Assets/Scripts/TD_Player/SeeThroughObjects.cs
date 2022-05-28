using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughObjects : MonoBehaviour
{
    //When something with, this script, enters a trigger with the tag "SeeThrough"
    //The object with that tag's sprite render opacity is reduced 
    //When that something leaves the opacity returns to normal
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("SeeThrough"))
        {
            SpriteRenderer spriteRenderer = collider2D.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }

        }
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("SeeThrough"))
        {
            SpriteRenderer spriteRenderer = collider2D.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
}
