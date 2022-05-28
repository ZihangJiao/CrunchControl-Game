using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUsageTracker : MonoBehaviour
{
    static public float doorTimer = 5;
    static public float doorWait = 1;

    void Update()
    {
        doorTimer += 1 * Time.deltaTime;
        if(doorTimer >= 1)
        {
            gameObject.tag = "Player";
            //Once the door timer reaches 1 the player's tag is returned to player

        }
    }
}
