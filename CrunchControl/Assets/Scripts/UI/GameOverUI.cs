using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameOverUI : MonoBehaviour
{
    //This code makes it so that when set to active, the game over screen slowly fades to balck before returning to the main menu

    public float timer;
    public Image image;
    public int type; // 0 indicate change to map scene, 1 indicate change to menu scene, 2 indicate dirst and second escape text

    void Start()
    {
        timer = 0;
    }

    void FixedUpdate()
    {
        if (type != 2)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (0.5f * Time.deltaTime));
        }
        timer += Time.deltaTime;

        if(timer >= 3 && type == 2)
        {
            gameObject.SetActive(false);
        }

        if (timer >= 2.5 && type == 0)
        {
            SceneManager.LoadScene("TopDownScene");
        }

        if (timer >= 5 && type == 1)
        {
            //string filepath = Application.dataPath + "/GameData/enemySaveData.json";
            string filepath = Application.dataPath + "/StreamingAssets.json";
            if ( File.Exists(filepath)){
                File.Delete(filepath);
            }
            LoadSceneManager.HP = 5;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
            SceneManager.LoadScene("MainMenu");
            
        }
    }
}
