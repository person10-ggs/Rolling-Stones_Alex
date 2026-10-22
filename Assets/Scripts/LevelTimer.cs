
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool timerRunning = true;

    private string levelName;
    private float bestTime;

    private void Start()
    {
        levelName = SceneManager.GetActiveScene().name;

        bestTime = PlayerPrefs.GetFloat(
            "BestTime_" + levelName, 0f);
    }

    private void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;

        Debug.Log("Timer reset after respawn!");
    }

    public void StopTimer()
    {
        if (!timerRunning)
            return;

        timerRunning = false;

        if (bestTime == 0f || elapsedTime < bestTime)
        {
            bestTime = elapsedTime;

            PlayerPrefs.SetFloat(
                "BestTime_" + levelName, bestTime);

            PlayerPrefs.Save();

            Debug.Log("NEW BEST TIME!");
        }

        Debug.Log("Level completed in: " +
            FormatTime(elapsedTime));
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt(
            (time * 1000f) % 1000f);

        return string.Format(
            "{0:00}:{1:00}.{2:000}",
            minutes, seconds, milliseconds);
    }

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
