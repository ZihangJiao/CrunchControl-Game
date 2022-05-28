using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class TaskManager : MonoSingleton<TaskManager>
{
    public GameObject[] tasks;

    public int printerCounter;
    public bool paperWorkCompleted = false;
    public int desklCounter;

    public TextMeshProUGUI printerText;
    public TextMeshProUGUI desklText;
    public TextMeshProUGUI printerTitle;
    public TextMeshProUGUI desklTitle;
    public TextMeshProUGUI paperworkTitle;
    public TextMeshProUGUI Return;


    public Image paperWorkTick;
    public Image paperWorkBox;

    public int completedTasksCounter;

    public GameObject bossEnemy;
    public Transform bossSpawn;

    public TextMeshProUGUI Win;

    public GameObject winBigger;

    private void Start()
    {
        paperWorkTick.enabled = false;
        paperWorkBox.enabled = true;
        UpdateUI();
    }

    public void UpdateTasks(int taskType)
    {  
                // Updates appropiate counter for the completed task
                if(taskType == 1)
                {
                    desklCounter += 1;
                }

                else if (taskType == 2)
                {
                    printerCounter += 1;
                }

                else if(taskType == 3)
                {
                    paperWorkCompleted = true;
                }

        UpdateUI();
/*
         completedTasksCounter += 1;
        if (completedTasksCounter == 5)
        {
            AllTasksCompleted();
        }*/
    }

    public void UpdateUI()
    {

        printerText.text = printerCounter + "/2"; 
        
        desklText.text =  desklCounter + "/2";

        if(paperWorkCompleted == true)
        {
            paperWorkTick.enabled = true;
            paperWorkBox.enabled = false;
        }


        //finds the total number of tasks completed and updates the UI to reflect the completed tasks
        if (paperWorkCompleted == true)
        {
            completedTasksCounter = desklCounter + printerCounter + 1;
        }
        else
        {
            completedTasksCounter = desklCounter + printerCounter;
        }
        //If all tasks are completed  call AllTasksCompleted
        if (completedTasksCounter == 5)
        {
            AllTasksCompleted();
        }
    }
    
    private void AllTasksCompleted()
    {
        if (LoadSceneManager.boss_defeated == false)
        {
            // once all tasks are complete spawn boss & update ui
            GameObject boss = Instantiate(bossEnemy, bossSpawn);
        }
        //LoadSceneManager.Instance.EnemyObjList.Add(boss);
        
        printerText.enabled     = false;
        desklText.enabled       = false;
        paperWorkTick.enabled   = false;
        paperWorkBox.enabled    = false;
        desklTitle.enabled      = false;
        printerTitle.enabled    = false;
        paperworkTitle.enabled  = false;
        Return.enabled          = true;
        Return.text = "Return to the Lifts";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && completedTasksCounter == 5)
        {
            StartCoroutine(WaitToEnd());
            winBigger.SetActive(true);
        }
    }

    IEnumerator WaitToEnd()
    {
        Win.enabled = true;

         yield return new WaitForSeconds(6f);


        string filepath = Application.dataPath + "/StreamingAssets.json";

        if (File.Exists(filepath))
        {
            StreamReader sr = new StreamReader(filepath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            Save save = JsonUtility.FromJson<Save>(jsonStr);
            save.health = LoadSceneManager.HP;
            string saveJsonStr = JsonUtility.ToJson(save);
            try
            {
                File.WriteAllText(filepath, saveJsonStr);

#if UNITY_EDITOR
                Debug.Log($"Successfully saved data to {filepath}.");
#endif
            }
            catch (System.Exception exception)
            {
#if UNITY_EDITOR
                Debug.LogError($"Failed save data to {filepath}. \n{exception}");
#endif
            }
        }


        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

