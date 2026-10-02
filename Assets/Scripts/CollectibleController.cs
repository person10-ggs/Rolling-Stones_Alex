/***
 * COMPONENTS OF: Collectibles Prefabs
 * REQUIRED DEPENDENCIES: GameManager, Collider Trigger and Particle System components
 * DESCRIPTION: Updated to include the new rotating behavior alongside the trigger collision logic.
 * AUTHOR: AChobantonov
 * VERSION: 1.1
 * RELEASE NOTES VERSION 1.2: Nothing Yet
***/

using UnityEngine;

public class CollectibleController : MonoBehaviour
{
[SerializeField] private AudioClip collectSound;
[SerializeField] private GameObject collectParticlePrefab;

// Required Data: Float value representing rotation speed in degrees per second (e.g., 50f)
[SerializeField] private float rotationSpeed = 50f;

private GameManager gameManager;

// Start is called once before the first execution of Update after the MonoBehaviour is created
private void Start()
{
    gameManager = FindAnyObjectByType<GameManager>();
}

// Update is called once per frame
private void Update()
{
    // Delegates frame rotation logic to its dedicated method
    RotateCollectible();
}

// Rotates the collectible continuously around its local Y-axis
private void RotateCollectible()
{
    // Applies smooth rotation around the Y-axis scaled by frame delta time
    transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
}

// Call with this object collides with a trigger
private void OnTriggerEnter(Collider other)
{
    // Only executes if the collision was with the Player
    if (other.CompareTag("Player"))
    {
        gameManager.UpdateRemaining();
        // Spawn audio at the collectible's position (auto-destroys)
        AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Spawn particles (auto-destroys if Stop Action is set to Destroy)
        Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

        // Safely destroy the collectible immediately
        Destroy(gameObject);
    }
}
}
