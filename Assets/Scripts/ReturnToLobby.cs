/*

GAMEOBJECT: Level Finish Portal / Return To Lobby Trigger

REQUIRED DEPENDENCIES: UnityEngine.SceneManagement

DESCRIPTION: Handles level completion when player reaches the finish portal, stops level timer, unlocks next level, awards completion coin to PlayerPrefs, and returns player to the lobby.

VERSION 1.1: Added coin award functionality upon level completion to persist earned coins in PlayerPrefs.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobby : MonoBehaviour
{
[Header("Level Settings")]
[Tooltip("The level number completed by reaching this portal.")]
public int completedLevel = 1;

[Header("Reward Settings")]
[Tooltip("Amount of coins rewarded to player upon completing the level. Requires an Integer value.")]
[SerializeField] private int coinRewardAmount = 1;

// Constant key matching the saved coin data in PlayerPrefs used by CoinDisplay and SkinManager
private const string CoinSaveKey = "PlayerCoins";

// Unity trigger execution when another collider enters this trigger zone
private void OnTriggerEnter(Collider other)
{
    ProcessLevelCompletion(other);
}

// Processes all level completion actions when the player enters the trigger
private void ProcessLevelCompletion(Collider other)
{
    // Verify colliding object is tagged as Player
    if (!other.CompareTag("Player"))
    {
        return;
    }

    // Execute modular level finish steps
    StopLevelTimer();
    UnlockNextLevel(other);
    AwardCompletionCoin();
    LoadLobbyScene();
}

// Stops the active level timer and saves best completion time
private void StopLevelTimer()
{
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
}

// Unlocks the subsequent level via LevelUnlockSystem attached to the Player
private void UnlockNextLevel(Collider player)
{
    LevelUnlockSystem unlockSystem = player.GetComponent<LevelUnlockSystem>();

    if (unlockSystem != null)
    {
        unlockSystem.UnlockLevel(completedLevel + 1);
        Debug.Log("Unlocked level " + (completedLevel + 1));
    }
    else
    {
        Debug.LogError("LevelUnlockSystem not found on Player!");
    }
}

// Increments total saved coin count in PlayerPrefs persistent storage
private void AwardCompletionCoin()
{
    // Retrieve existing coin count from PlayerPrefs disk storage (defaults to 0 if key does not exist)
    int currentCoins = PlayerPrefs.GetInt(CoinSaveKey, 0);

    // Calculate updated coin sum
    int updatedCoins = currentCoins + coinRewardAmount;

    // Persist new coin total to disk storage
    PlayerPrefs.SetInt(CoinSaveKey, updatedCoins);
    PlayerPrefs.Save();

    Debug.Log("Level Complete! Awarded " + coinRewardAmount + " coin(s). Total Coins: " + updatedCoins);
}

// Loads the main lobby scene
private void LoadLobbyScene()
{
    SceneManager.LoadScene("Lobby");
}
}