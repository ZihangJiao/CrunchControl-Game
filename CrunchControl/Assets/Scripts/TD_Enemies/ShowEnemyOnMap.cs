using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ShowEnemyOnMap : MonoBehaviour
{
    public EnemyOnMap enemy;
    public int alive;
    public int id;
    public int difficulty_level; // 0 = tutorial enemy, 1 = easy enemy, 2 = medium enemy, 3 = hard enemy, 4 = boss enemy
    public float posx;
    public float posy;
    public float posz;

    // Start is called before the first frame update
    void Start()
    {
        enemy = new EnemyOnMap(id, alive);


        LoadByJson();

    }

    // Update is called once per frame
    void Update()
    {
        
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
        for (int i = 0; i < save.ID.Count; i++)
        {
            if (save.ID[i] == enemy.id)
            {
                /*  Vector3 tempPos = new Vector3(save.Pos_x[i], save.Pos_y[i], save.Pos_y[i]);
                  enemy.transform.parent.position = tempPos;*/

                if (save.EnemyLiveStatus[i] == 0)
                {
                    enemy.alive = 0;
                    transform.parent.gameObject.SetActive(false);
                    break;
                }
                Vector3 tempPos = new Vector3(save.Pos_x[i], save.Pos_y[i], 0);
                transform.parent.position = tempPos;
                break;
            }
        }
    }

}
