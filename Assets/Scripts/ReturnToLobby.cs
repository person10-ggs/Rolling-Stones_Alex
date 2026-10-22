
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{
    public int completedLevel = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Stop the timer and save the best time
        LevelTimer timer = FindFirstObjectByType<LevelTimer>();

        if (timer != null)
        {
            timer.StopTimer();
            Debug.Log("Level timer stopped!");
        }
        else
        {
            Debug.LogWarning("LevelTimer not found in this scene!");
        }

        // Unlock the next level
        LevelUnlockSystem unlockSystem =
            other.GetComponent<LevelUnlockSystem>();

        if (unlockSystem != null)
        {
            unlockSystem.UnlockLevel(completedLevel + 1);
            Debug.Log("Unlocked level " + (completedLevel + 1));
        }
        else
        {
            Debug.LogError("LevelUnlockSystem not found on Player!");
        }

        // Return to the lobby
        SceneManager.LoadScene("Lobby");
    }
}
