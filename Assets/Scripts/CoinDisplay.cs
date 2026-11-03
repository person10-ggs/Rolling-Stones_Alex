/*
 * GAMEOBJECT: UI Canvas / Coin Display Text
 * REQUIRED DEPENDENCIES: TMPro.TextMeshProUGUI, UnityEngine.InputSystem
 * DESCRIPTION: Manages persistent top-right UI coin display across scene reloads and player respawns using stored PlayerPrefs data, and processes keyboard cheat inputs using the Unity New Input System exclusively.
 * VERSION 1.2: Removed legacy Input fallback to resolve InputSystem package conflict for C key cheat input.
 */

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    // Serialized field to hold reference to the TextMeshPro UI element in the Inspector
    [Header("UI Component Settings")]
    [Tooltip("Assign the TextMeshProUGUI element responsible for displaying coin count.")]
    [SerializeField] private TextMeshProUGUI coinTextUI;

    [Header("Cheat Settings")]
    [Tooltip("Integer value representing how many coins to award when pressing the cheat key.")]
    [SerializeField] private int cheatCoinAmount = 5;

    // Constant key matching the saved coin data in PlayerPrefs used by GameManager and SkinManager
    private const string CoinSaveKey = "PlayerCoins";

    // Integer variable tracking the last displayed count to prevent redundant text re-renders
    private int lastDisplayedCoins = -1;

    // Unity Start method called once when script is initialized
    private void Start()
    {
        InitializeCoinDisplay();
    }

    // Unity Update method called once per frame - strictly delegates to helper methods
    private void Update()
    {
        RefreshCoinCount();
        ProcessCheatInput();
    }

    // Automatically caches attached TextMeshProUGUI component if left unassigned in Inspector
    private void InitializeCoinDisplay()
    {
        if (coinTextUI == null)
        {
            coinTextUI = GetComponent<TextMeshProUGUI>();
        }

        UpdateCoinDisplay();
    }

    // Frame-by-frame monitoring method delegated from Update
    private void RefreshCoinCount()
    {
        UpdateCoinDisplay();
    }

    // Monitors for cheat inputs during runtime - delegated from Update
    private void ProcessCheatInput()
    {
        CheckCheatKey();
    }

    // Checks if the C key was pressed using the Unity New Input System exclusively
    private void CheckCheatKey()
    {
        // Query active keyboard device through New Input System API
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            AddCheatCoins();
        }
    }

    // Adds the cheat coin amount to stored PlayerPrefs and saves to disk
    private void AddCheatCoins()
    {
        int currentCoins = PlayerPrefs.GetInt(CoinSaveKey, 0);
        int updatedCoins = currentCoins + cheatCoinAmount;

        PlayerPrefs.SetInt(CoinSaveKey, updatedCoins);
        PlayerPrefs.Save();

        Debug.Log("CHEAT ACTIVATED: Added " + cheatCoinAmount + " coins. Total: " + updatedCoins);
    }

    // Fetches saved coins from PlayerPrefs and updates UI text when coin balance changes
    private void UpdateCoinDisplay()
    {
        // Retrieve current saved coins from disk (defaults to 0 if key does not exist)
        int currentCoins = PlayerPrefs.GetInt(CoinSaveKey, 0);

        // Evaluate if the coin value changed since the last frame
        if (currentCoins != lastDisplayedCoins)
        {
            // Store current coin value to compare against future frames
            lastDisplayedCoins = currentCoins;

            // Verify UI text reference exists before attempting text assignment
            if (coinTextUI != null)
            {
                // Assign formatted text string to display current coin total
                coinTextUI.text = "Coins: " + currentCoins;
            }
        }
    }
}