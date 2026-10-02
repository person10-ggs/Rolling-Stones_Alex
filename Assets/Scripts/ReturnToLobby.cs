using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}