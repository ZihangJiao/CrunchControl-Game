using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlindSpot : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //print("Player stealth successful");


/*            LoadSceneManager.Instance.LoadNextScene();
            LoadSceneManager.Instance.SaveByJson();*/
        }
    }
}

