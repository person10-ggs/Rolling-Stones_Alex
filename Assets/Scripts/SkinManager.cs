/*
 * GAMEOBJECT: Player (Ball) / Gacha Manager Object
 * REQUIRED DEPENDENCIES: Renderer, UnityEngine.InputSystem
 * DESCRIPTION: Manages available skin materials, processes Gacha spins using Unity New Input System, deducts coins, and persists active equipped skin across scenes.
 * VERSION 1.1: Replaced legacy Input with UnityEngine.InputSystem for G key spin input to resolve active input settings conflict.
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class SkinManager : MonoBehaviour
{
    [Header("Skin Settings")]
    [Tooltip("Assign all available skin materials to be pulled from the Gacha machine.")]
    [SerializeField] private Material[] availableSkins;

    [Tooltip("Assign the MeshRenderer or Renderer component of the player object.")]
    [SerializeField] private Renderer targetRenderer;

    private const string SkinSaveKey = "EquippedSkinIndex";
    private const string CoinSaveKey = "PlayerCoins";

    private void Start()
    {
        InitializeSkinSystem();
    }

    private void Update()
    {
        ProcessInput();
    }

    // Caches target renderer and applies the currently saved skin material
    private void InitializeSkinSystem()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        ApplyEquippedSkin();
    }

    // Monitors for Gacha spin trigger input during runtime - delegated from Update
    private void ProcessInput()
    {
        CheckGachaSpinInput();
    }

    // Checks if the G key was pressed using Unity New Input System
    private void CheckGachaSpinInput()
    {
        // Query active keyboard device through New Input System API
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            SpinGacha();
        }
    }

    // Retrieves saved skin index from PlayerPrefs and applies the material to the Renderer
    public void ApplyEquippedSkin()
    {
        if (availableSkins == null || availableSkins.Length == 0 || targetRenderer == null)
        {
            return;
        }

        int savedIndex = PlayerPrefs.GetInt(SkinSaveKey, 0);

        // Clamps saved index to prevent out-of-bounds errors if array size changes
        savedIndex = Mathf.Clamp(savedIndex, 0, availableSkins.Length - 1);

        targetRenderer.material = availableSkins[savedIndex];
    }

    // Consumes 1 coin to select a random skin index, save it, and apply it immediately
    public bool SpinGacha()
    {
        int currentCoins = PlayerPrefs.GetInt(CoinSaveKey, 0);

        if (currentCoins < 1)
        {
            Debug.LogWarning("Gacha Spin Failed: You need at least 1 coin to spin!");
            return false;
        }

        if (availableSkins == null || availableSkins.Length == 0)
        {
            Debug.LogError("Gacha Spin Failed: No skin materials assigned in Inspector!");
            return false;
        }

        // Deduct 1 coin
        PlayerPrefs.SetInt(CoinSaveKey, currentCoins - 1);

        // Pick a random skin index from the assigned array
        int randomIndex = Random.Range(0, availableSkins.Length);

        // Overwrite saved skin with new random result
        PlayerPrefs.SetInt(SkinSaveKey, randomIndex);
        PlayerPrefs.Save();

        // Immediately update player appearance
        ApplyEquippedSkin();

        Debug.Log("GACHA SPIN SUCCESS! New skin equipped (Index " + randomIndex + "). Remaining coins: " + (currentCoins - 1));
        return true;
    }

    // Returns current coin balance for UI displays
    public int GetCoinCount()
    {
        return PlayerPrefs.GetInt(CoinSaveKey, 0);
    }
}