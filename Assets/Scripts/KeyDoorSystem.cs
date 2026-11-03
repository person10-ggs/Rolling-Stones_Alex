/*

GameObject: Key / Collectible Object

Dependencies: Collider (IsTrigger = true)

Description: Manages key interaction with player, hides assigned door upon touch, and allows state reset upon player respawn.

VERSION 1.1: Replaced Destroy with SetActive(false) and added ResetKey method for respawn reset.
*/

using UnityEngine;

public class KeyDoorSystem : MonoBehaviour
{
public string keyTag = "Player";
public GameObject door;
public bool destroyKeyOnTouch = true;

private void Start()
{
    // Set initial door and key state at scene launch
    ResetKey();
}

private void OnTriggerEnter(Collider other)
{
    Debug.Log("Detected object: " + other.name +
        " | Tag: " + other.tag +
        " | Expected: " + keyTag);

    if (other.CompareTag(keyTag))
    {
        Debug.Log("KEY COLLECTED! Hiding door.");

        // Hide the door when the player touches the key
        if (door != null)
            door.SetActive(false);
        else
            Debug.LogError("DOOR IS NOT ASSIGNED!");

        // Hide the key object instead of destroying it so it can be restored on respawn
        if (destroyKeyOnTouch)
            gameObject.SetActive(false);
    }
}

// Resets the key object and associated door to their default active states
public void ResetKey()
{
    // Reactivate key GameObject in scene
    gameObject.SetActive(true);

    // Reactivate door GameObject if assigned
    if (door != null)
    {
        door.SetActive(true);
    }
}
}

/*

GameObject: Respawn Ground / Hazard Zone

Dependencies: Collider (IsTrigger = true)

Description: Respawns player on collision, resets velocity, level timer, and scene collectibles.

VERSION 1.1: Added ResetCollectibles method to reset all KeyDoorSystem items on respawn.
*/