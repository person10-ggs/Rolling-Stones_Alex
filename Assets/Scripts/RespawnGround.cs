using UnityEngine;

public class RespawnGround : MonoBehaviour
{
public Transform respawnPoint;

private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player"))
        return;

    if (respawnPoint == null)
    {
        Debug.LogError("Respawn Point is not assigned!");
        return;
    }

    // Respawn the player
    other.transform.position = respawnPoint.position;

    // Stop the ball's movement
    Rigidbody rb = other.GetComponent<Rigidbody>();

    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Reset the level timer
    LevelTimer timer = FindFirstObjectByType<LevelTimer>();

    if (timer != null)
    {
        timer.ResetTimer();
    }
    else
    {
        Debug.LogWarning("LevelTimer not found in this scene!");
    }

    // Reset all key/door collectibles in the scene
    ResetCollectibles();

    Debug.Log("Player respawned, timer reset, and collectibles reset!");
}

// Searches for all KeyDoorSystem components in scene and resets them
private void ResetCollectibles()
{
    // Locate all KeyDoorSystem instances, including inactive GameObjects
    KeyDoorSystem[] keys = FindObjectsByType<KeyDoorSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

    // Iterate through each key system and trigger reset functionality
    foreach (KeyDoorSystem key in keys)
    {
        if (key != null)
        {
            key.ResetKey();
        }
    }
}
}