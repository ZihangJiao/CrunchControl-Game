using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<int> ID = new List<int>();
    public List<int> EnemyLiveStatus = new List<int>();
    public List<float> Pos_x = new List<float>();
    public List<float> Pos_y = new List<float>();
    public float Player_Pos_x;
    public float Player_Pos_y;
    public int health;
    public int printerCounter;
    public int desklCounter;
    public int paperWorkCompleted;
    public List<int> TaskObjList = new List<int>();
}
