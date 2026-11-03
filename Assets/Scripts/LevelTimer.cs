/*

GAMEOBJECT: Level Timer / Manager Object

REQUIRED DEPENDENCIES: UnityEngine.SceneManagement

DESCRIPTION: Tracks level completion time, updates HUD displays, and saves high scores tied directly to the player's unique hardware device code.

VERSION 1.1: Tied PlayerPrefs save keys to SystemInfo.deviceUniqueIdentifier to store individual device best times.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
private float elapsedTime = 0f;
private bool timerRunning = true;

private string levelName;
private string deviceId;
private float bestTime;

private void Start()
{
    // Cache active scene name for level-specific key identification
    levelName = SceneManager.GetActiveScene().name;

    // Retrieve unique hardware identifier for current device
    deviceId = SystemInfo.deviceUniqueIdentifier;

    // Load device-specific best completion time from PlayerPrefs
    bestTime = PlayerPrefs.GetFloat(GetSaveKey(), 0f);
}

private void Update()
{
    // Delegates frame timing updates to dedicated helper method
    TrackElapsedTime();
}

// Increments total elapsed time while timer remains active
private void TrackElapsedTime()
{
    if (timerRunning)
    {
        elapsedTime += Time.deltaTime;
    }
}

// Resets current level timer back to zero upon player respawn
public void ResetTimer()
{
    elapsedTime = 0f;
    timerRunning = true;

    Debug.Log("Timer reset after respawn!");
}

// Stops level timer and updates device-bound high score if record is broken
public void StopTimer()
{
    if (!timerRunning)
        return;

    timerRunning = false;

    // Check if no previous record exists or if current time beats record
    if (bestTime == 0f || elapsedTime < bestTime)
    {
        bestTime = elapsedTime;

        // Save record using unique device key
        PlayerPrefs.SetFloat(GetSaveKey(), bestTime);

        PlayerPrefs.Save();

        Debug.Log("NEW BEST TIME!");
    }

    Debug.Log("Level completed in: " + FormatTime(elapsedTime));
}

// Helper method to generate PlayerPrefs key binding level name with device ID
private string GetSaveKey()
{
    return "BestTime_" + levelName + "_" + deviceId;
}

// Formats raw time in seconds into readable MM:SS.FFF format string
private string FormatTime(float time)
{
    int minutes = Mathf.FloorToInt(time / 60f);
    int seconds = Mathf.FloorToInt(time % 60f);
    int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

    return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
}

// Draws temporary debug HUD interface for time display
private void OnGUI()
{
    GUIStyle style = new GUIStyle();
    style.fontSize = 26;
    style.normal.textColor = Color.white;
    style.fontStyle = FontStyle.Bold;

    GUI.Label(
        new Rect(20, 20, 400, 40),
        "Time: " + FormatTime(elapsedTime),
        style);

    string bestTimeText = bestTime > 0f
        ? FormatTime(bestTime)
        : "--:--.---";

    GUI.Label(
        new Rect(20, 60, 450, 40),
        "Best: " + bestTimeText,
        style);

    if (!timerRunning)
    {
        GUI.Label(
            new Rect(20, 100, 400, 40),
            "Level Complete!",
            style);
    }
}
}