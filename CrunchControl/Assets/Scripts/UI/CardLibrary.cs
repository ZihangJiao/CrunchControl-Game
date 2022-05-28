using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public Vector3 originSize;

    void Start()
    {
        originSize = gameObject.transform.localScale;
    }

    public void EnlargeCard()
    {
        gameObject.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f); 
    }

    public void ShrinkCard()
    {
        gameObject.transform.localScale = originSize;
    }
}
