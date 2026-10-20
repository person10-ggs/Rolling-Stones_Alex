
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{
    public int completedLevel = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

            SceneManager.LoadScene("Lobby");
        }
    }
}
