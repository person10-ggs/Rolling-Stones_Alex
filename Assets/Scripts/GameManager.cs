/*

GAMEOBJECT: Game Manager

REQUIRED DEPENDENCIES: TextMeshProUGUI, AudioSource

DESCRIPTION: Tracks level collectibles, handles win sequence, awards level completion coins via PlayerPrefs, and reloads the level.

VERSION 1.1: Added coin reward logic triggered upon level win to feed the Gacha skin system.
*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
[SerializeField] private TextMeshProUGUI RemainingTextUI;
[SerializeField] private AudioClip winClip;

private AudioSource audioSource;
private int numberOfCollectibles;

private const string CoinSaveKey = "PlayerCoins";

private void Start()
{
    InitializeManager();
}

private void Update()
{
    // Update reserved for delegated method execution only
}

// Configures audio source and counts total collectibles present at level launch
private void InitializeManager()
{
    audioSource = GetComponent<AudioSource>();
    numberOfCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
    UpdateRemainingUI();
}

// Decrements remaining collectible count when triggered by collectible items
public void UpdateRemaining()
{
    numberOfCollectibles--;
    UpdateRemainingUI();
}

// Updates HUD text or initiates end game routine when all collectibles are gathered
private void UpdateRemainingUI()
{
    if (RemainingTextUI != null && numberOfCollectibles > 0)
    {
        RemainingTextUI.text = "Collectibles Remaining: " + numberOfCollectibles;
    }
    else
    {
        StartCoroutine(EndGame());
    }
}

// Handles victory audio playback, coin awarding, and scene reload delay
private IEnumerator EndGame()
{
    if (RemainingTextUI != null)
    {
        RemainingTextUI.text = "You Win!";
    }

    if (audioSource != null && winClip != null)
    {
        audioSource.clip = winClip;
        audioSource.Play();
    }

    // Award 1 coin for completing the level
    AwardCompletionCoin();

    if (audioSource != null && audioSource.clip != null)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
    }
    else
    {
        yield return new WaitForSeconds(1f);
    }

    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.name);
}

// Increments total saved coin count in PlayerPrefs persistent storage
private void AwardCompletionCoin()
{
    int currentCoins = PlayerPrefs.GetInt(CoinSaveKey, 0);
    PlayerPrefs.SetInt(CoinSaveKey, currentCoins + 1);
    PlayerPrefs.Save();

    Debug.Log("Level Win! Coin awarded. Total Coins: " + (currentCoins + 1));
}
}

/*

GAMEOBJECT: Player (Ball) / Gacha Manager Object

REQUIRED DEPENDENCIES: Renderer

DESCRIPTION: Manages available skin materials, processes Gacha spins, deducts coins, and persists active equipped skin across scenes.

VERSION 1.0: Created Gacha Skin system script handling coin expenditure, random skin selection, and persistent skin application.
*/