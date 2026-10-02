using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 15f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Use the trampoline's upward direction.
                Vector3 bounceDirection = transform.up;

                // Remove the player's current upward velocity.
                playerRb.linearVelocity = new Vector3(
                    playerRb.linearVelocity.x,
                    0f,
                    playerRb.linearVelocity.z
                );

                // Bounce in the direction the trampoline is facing.
                playerRb.AddForce(
                    bounceDirection * bounceForce,
                    ForceMode.Impulse
                );
            }
        }
    }
}