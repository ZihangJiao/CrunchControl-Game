using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Task : MonoBehaviour
{
    public int taskType; //1 - Deskl, 2 - Printer Jam (Printer),  - Paperwork (Desk3), 

    public float taskProgress = 0f;
    public float progressIncrease = 30f;
    public float progressGoal = 100f;

    public bool isTaskComplete = false;
}
