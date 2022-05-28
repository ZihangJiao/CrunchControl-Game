using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float enemyMovementSpeed;
    public float enemyRotationSpeed;
    public Transform[] patrolPoints;
    public int currentPoint;
    public float waitTime;
    public float enemyDistancePoint;
    public float roundingDistance;

    public bool once;
    public bool reverse = false;

    public bool loopedPatrol;

    public Animator enemyAnimator;

    public GameObject enemySprite;
    public TextAsset DeadStatusData;


    void Update()
    {
        // Controls whether the the enenmy loops back to the first patrol point
        // when it reaches the end of the patrol points array. Or if the enemy goes
        // Through the array backwards.
        if(loopedPatrol == true)
        {
            LoopPatrol();
        }
        else
        {
            ReversePatrol();
        }
    }

    private void ReversePatrol()
    {
        //Moves the enemy towards the current patrol point while
        //tracking the distance between them
        if (transform.position != patrolPoints[currentPoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                                                     patrolPoints[currentPoint].position, 
                                                     enemyRotationSpeed * Time.deltaTime);
           
            enemyDistancePoint = Vector2.Distance(transform.position, 
                                                  patrolPoints[currentPoint].position);
        }
        else
        {
            if (!once)
            {
                once = true;

                StartCoroutine(ReverseWait());
            }
        }
    }

    IEnumerator ReverseWait()
    {
        //After waiting for a decided amount checks whether the enemy
        //is going forwards or backwards through the array
        enemyAnimator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(waitTime);

        CheckDirection();
        //If forwards
        if (reverse == false)
        {
            if (currentPoint + 1 < patrolPoints.Length)
            {
                currentPoint++;
            }
        } // If backwards
        else if (reverse == true)
        {
            if (currentPoint - 1 > -1)
            {
                currentPoint--;
            }
        }
         once = false;
        // Rotates Enemy field of view
        RotateTowardsPoint();

        enemyAnimator.SetFloat("Speed", 1);
    }

    private void CheckDirection()
    {
        //Checks if the enemy patrol point is at ethier the last of first patrol point
        //if changes the reverse boolean to reflect it
        if(enemyDistancePoint <= roundingDistance)
        {
            if(currentPoint == patrolPoints.Length - 1)
            {
                reverse = true;
            }
            else if(currentPoint == 0)
            {
                reverse = false;
            }
        } 
    }

    private void LoopPatrol()
    {
        //Moves the enemy towards the current patrol point 
        if (transform.position != patrolPoints[currentPoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                                                    patrolPoints[currentPoint].position, 
                                                    enemyMovementSpeed * Time.deltaTime);
            
        }
        else
        {
            if (!once)
            {
                once = true;
                StartCoroutine(LoopWait());
            }
        }
    }

    IEnumerator LoopWait()
    {
        // After waiting for a decided amount changes the current
        // patrol point  to the next one in the array or sets it to
        // the first one in it
        enemyAnimator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(waitTime);

        if (currentPoint + 1 < patrolPoints.Length)
        {
            currentPoint++;
        }
        else
        {
            currentPoint = 0;
        }

        once = false;

        //rotates enemy field of view
        RotateTowardsPoint();

        enemyAnimator.SetFloat("Speed", 1);
    }

    private void RotateTowardsPoint()
    {
        // Finds the angle between the enemy position and current patrol point
        // applies that to the first child of the enemy which is the enemy's fov
        // and to the enemies animations
        Vector2 direction = patrolPoints[currentPoint].position - transform.position;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(directionAngle - 90, Vector3.forward);
        Transform child = transform.GetChild(0);
        child.transform.rotation = rotation;

        enemyAnimator.SetFloat("Direction", directionAngle - 90);
    }

/*    public void ReadDeadStatus()
    {
        string[] dataRow = DeadStatusData.text.Split('\n');//read from the data that state whether the enemy is dead
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if(rowArray[0] == "id" || rowArray[0] == "")
            {
                continue;
            }
            else if (int.Parse(rowArray[0]) == id)
            {
                if (int.Parse(rowArray[1]) == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                 
                initial_position = new Vector3(float.Parse(rowArray[2]), float.Parse(rowArray[3]), float.Parse(rowArray[4]));
}                transform.position = initial_position;
            }
        }
    }*/
    

}
