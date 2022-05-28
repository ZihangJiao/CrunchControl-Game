using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public GameObject LoadScreen;
    public Slider slider;
    public Text text;

    public List<GameObject> EnemyObjList;
    public GameObject Player;

    public static bool isStealthBattle;
    public static int difficulty_level;
    public static int HP = 5;
    public static bool boss_defeated;

    public GameObject HP_list;
    public GameObject PauseMenu;
    public GameObject Task;

    public List<GameObject> TaskObjList;

    void Start()
    {



        //string filepath = Application.dataPath + "/GameData/enemySaveData.json";
        string filepath = Application.dataPath + "/StreamingAssets.json";

        if (File.Exists(filepath))
        {
            StreamReader sr = new StreamReader(filepath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            Save save = JsonUtility.FromJson<Save>(jsonStr);
            Vector3 tempPos = new Vector3(save.Player_Pos_x, save.Player_Pos_y, 0);
            Player.transform.position = tempPos;
            HP = save.health;
            int printerCounter = save.printerCounter;
            int desklCounter = save.desklCounter;
            int paperWorkCompleted = save.paperWorkCompleted;
            TaskManager.Instance.printerCounter = printerCounter;
            TaskManager.Instance.desklCounter = desklCounter;
            if (paperWorkCompleted == 1)
            {
                TaskManager.Instance.paperWorkCompleted = true;
            }
            else
            {
                TaskManager.Instance.paperWorkCompleted = false;
            }
            TaskManager.Instance.UpdateUI();

            for(int i = 0; i < 5; i++)
            {
                if(save.TaskObjList[i] == 1)
                {
                    TaskObjList[i].GetComponent<TaskInteract>().isTaskComplete = true;
                }
                else
                {
                    TaskObjList[i].GetComponent<TaskInteract>().isTaskComplete = false;
                }
            }
        }
           


        if (HP >= 0)
        {
            for (int i = 0; i < 5 - HP; i++)
            {
                Destroy(HP_list.transform.GetChild(i).gameObject);
            }
        }


    }


        public void LoadNextScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        LoadScreen.SetActive(true);// show loading background
        PauseMenu.SetActive(false);
        Task.SetActive(false);
        AsyncOperation operation = SceneManager.LoadSceneAsync("CardBattle");

        //operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {

            slider.value = operation.progress;

            text.text = operation.progress * 100 + "%";

            if(operation.progress >= 0.8f)
            {
                slider.value = 1;
                text.text = operation.progress * 100 + "%";
            }

            yield return null;
        }
    }
    public Save CreateSaveData()
    {
        Save save = new Save();
        foreach (GameObject enemy in EnemyObjList)
        {
            EnemyOnMap enemyOnMap = enemy.GetComponent<ShowEnemyOnMap>().enemy;

            save.EnemyLiveStatus.Add(enemyOnMap.alive);
            save.ID.Add(enemyOnMap.id);
            save.Pos_x.Add(enemy.transform.parent.transform.position.x);
            save.Pos_y.Add(enemy.transform.parent.transform.position.y);
        }
        save.Player_Pos_x = Player.transform.position.x;
        save.Player_Pos_y = Player.transform.position.y;
        save.health = HP;
        save.printerCounter = TaskManager.Instance.printerCounter;
        save.desklCounter = TaskManager.Instance.desklCounter;
        if (TaskManager.Instance.paperWorkCompleted == true)
        {
            save.paperWorkCompleted = 1;
        }
        else
        {
            save.paperWorkCompleted = 0;
        }
        for (int i = 0; i < 5; i++)
        {
            if (TaskObjList[i].GetComponent<TaskInteract>().isTaskComplete)
            {
                save.TaskObjList.Add(1);
            }
            else
            {
                save.TaskObjList.Add(0);

            }

        }
        return save;
    }

    public void SaveByJson()
    {
        Save save = CreateSaveData();
        //string filepath = Application.dataPath + "/GameData/enemySaveData.json";
        string filepath = Application.dataPath + "/StreamingAssets.json";
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




    public void LoadByJson()
    {
        //string filepath = Application.dataPath + "/GameData/enemySaveData.json";
        string filepath = Application.dataPath + "/StreamingAssets.json";

        if (File.Exists(filepath))
        {
            StreamReader sr = new StreamReader(filepath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            Save save = JsonUtility.FromJson<Save>(jsonStr);
            SetEnemy(save);
        }
    }
    public void SetEnemy(Save save)
    {
        foreach (GameObject enemy in EnemyObjList)
        {
            int found = 0;

            for (int i = 0; i < save.ID.Count; i++)
            {

                if (save.ID[i] == enemy.GetComponent<ShowEnemyOnMap>().enemy.id)
                {


                    if (save.EnemyLiveStatus[i] == 0)
                    {
                        enemy.GetComponent<ShowEnemyOnMap>().enemy.alive = 0;
                        enemy.transform.parent.gameObject.SetActive(false);
                    }
                    found = 1;
                    break;
                }
            }
            if (found == 0)
            {
                enemy.GetComponent<ShowEnemyOnMap>().enemy.alive = 0;
                enemy.transform.parent.gameObject.SetActive(false);
            }
            

        }

        
    }

}
