
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

        Debug.Log("Player respawned and timer reset!");
    }
}
