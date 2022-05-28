using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR 
using UnityEditor;
#endif


public class EnemyPlayerDetection : MonoBehaviour
{
    public float viewRadius = 5f;
    [RangeAttribute(1,360)] public float viewAngle = 45f;

    public LayerMask playerLayer;
    public LayerMask environmentLayer;

    public GameObject playerRef;

    public GameObject enemy;
    public GameObject enemySprite;

    public bool CanSeePlayer;

    private void Start()
    {
        StartCoroutine(EnemyFOVCheck());
    }

    private IEnumerator EnemyFOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            EnemyFOV();
        }
    }

    private void EnemyFOV()
    {
        // Creates an array of all colliders on the player Layer ovellapping a circle around the enemy
        Collider2D[] fovCheck = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        if(fovCheck.Length > 0)
        {
            // Finds the direction to the player
            Transform target = fovCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            // checks if the player is in the enemies field of view 
            if(Vector2.Angle(transform.up, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                //Check's if the enemy was direct sight of the player.
                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, environmentLayer) && CanSeePlayer != true)
                {
                    CanSeePlayer = true;
                  //print("Enemy can see the player");
                    // ^^^ Load card battler scene 

                    LoadSceneManager.isStealthBattle = false;
                    LoadSceneManager.difficulty_level = enemySprite.GetComponent<ShowEnemyOnMap>().difficulty_level;

                    if (enemySprite.GetComponent<ShowEnemyOnMap>().enemy.id == 99)
                    {
                        LoadSceneManager.boss_defeated = true;
                        Destroy(enemySprite);
                    }

                    LoadSceneManager.Instance.LoadNextScene();
                    enemySprite.GetComponent<ShowEnemyOnMap>().enemy.alive = 0;
                    LoadSceneManager.Instance.SaveByJson();


                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        //draws the enemie's field of view 
        Gizmos.color = Color.magenta;

#if UNITY_EDITOR
        Handles.DrawWireDisc(transform.position, Vector3.forward, viewRadius);
#endif
        Vector3 LineA = DirectionFromAngle(-transform.eulerAngles.z, -viewAngle / 2);
        Vector3 LineB = DirectionFromAngle(-transform.eulerAngles.z, viewAngle / 2);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + LineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + LineB * viewRadius);
        
        //Draws line to player
        if (CanSeePlayer)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }

    }
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Player stealth successful");

            LoadSceneManager.isStealthBattle = true;
            LoadSceneManager.difficulty_level = enemySprite.GetComponent<ShowEnemyOnMap>().difficulty_level;

            enemySprite.GetComponent<ShowEnemyOnMap>().enemy.alive = 0;
            if (enemySprite.GetComponent<ShowEnemyOnMap>().enemy.id == 99)
            {
                LoadSceneManager.boss_defeated = true;
                Destroy(enemySprite);
            }

            LoadSceneManager.Instance.LoadNextScene();
            LoadSceneManager.Instance.SaveByJson();


        }
    }
}

