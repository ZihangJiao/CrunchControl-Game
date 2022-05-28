using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUsage : MonoBehaviour
{
    public Transform destinationPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If a player enters the door's trigger And the door timer is higher than
        // than the door wait value the player is moved to the linked door
        // then the player's tag changes to prevent the door from immeditately activating
        // then the door timer is reset to zero
        if (collision.tag == "Player" && DoorUsageTracker.doorTimer >= DoorUsageTracker.doorWait)
        {
            collision.transform.position = destinationPosition.position;
            collision.gameObject.tag = "Finish";

            DoorUsageTracker.doorTimer = 0f;
        }
    }
}

