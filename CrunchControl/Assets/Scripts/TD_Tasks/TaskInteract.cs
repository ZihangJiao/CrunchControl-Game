using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskInteract : Task
{
    public TaskManager taskManager;

    public Slider progressBar;
    public Image border;
    public Image fill;

    public Material secondaryMat;

    public void Start()
    {
        progressBar.enabled = false;
        border.enabled = false;
        fill.enabled = false;

        progressBar.value = 0;

        if(isTaskComplete == true)
        {
            GetComponent<SpriteRenderer>().material = secondaryMat; 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // When player enters task trigger the progress tracker is enabled
        if (collision.tag == "Player" && isTaskComplete == false)
        {
            progressBar.enabled = true;
            border.enabled = true;
            fill.enabled = true;

            // When player interacts the progress value increases
            if (Input.GetKey(KeyCode.Space))
            {
                taskProgress += progressIncrease * Time.deltaTime;

                progressBar.value = taskProgress;

                // When the task is complete the update tasks function is called
                // and the progress bar is reset. Finally the material is set to it's default value
                if (taskProgress >= progressGoal)
                {
                    isTaskComplete = true;

                    taskManager.UpdateTasks(taskType);

                    GetComponent<SpriteRenderer>().material = secondaryMat;

                    progressBar.value = 0;

                    progressBar.enabled = false;
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        progressBar.enabled = false;
        border.enabled = false;
        fill.enabled = false;
    }

}