/*
 * GAMEOBJECT: MusicManager (Persistent Object across Scenes)
 * REQUIRED DEPENDENCIES: AudioSource
 * DESCRIPTION: Manages persistent background music playback across all levels, automatically cycling through a playlist in sequential order without stopping or resetting between level loads.
 * VERSION 1.0: Implemented persistent Singleton music player that cycles audio tracks across scene changes without needing extra scene setup.
 */

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    // Static reference to the single allowed instance of MusicManager
    private static MusicManager instance;

    [Header("Playlist Settings")]
    [Tooltip("Assign 5 background music audio clips to play sequentially across scenes.")]
    [SerializeField] private AudioClip[] playlist;

    // Component reference and tracking variables
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    // Called when the script instance is being loaded
    private void Awake()
    {
        SetupSingleton();
    }

    // Called before the first frame update
    private void Start()
    {
        InitializeAudio();
    }

    // Called once per frame
    private void Update()
    {
        MonitorTrackProgress();
    }

    // Ensures only one instance of MusicManager exists across scene loads
    private void SetupSingleton()
    {
        if (instance != null && instance != this)
        {
            // Destroy duplicate manager created when re-entering initial scene
            Destroy(gameObject);
            return;
        }

        // Assign current instance and preserve across scene switches
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Caches AudioSource reference and initiates playlist playback
    private void InitializeAudio()
    {
        audioSource = GetComponent<AudioSource>();

        // Disable clip looping so track end events can be detected
        audioSource.loop = false;

        // Start playback if valid audio clips exist in array
        if (playlist != null && playlist.Length > 0)
        {
            PlayCurrentTrack();
        }
        else
        {
            Debug.LogWarning("MusicManager: No audio clips assigned to playlist!");
        }
    }

    // Delegates track completion monitoring to ensure continuous playback
    private void MonitorTrackProgress()
    {
        // Check if current clip has finished playing and playlist is valid
        if (audioSource != null && !audioSource.isPlaying && playlist != null && playlist.Length > 0)
        {
            AdvanceToNextTrack();
        }
    }

    // Advances track index sequentially and wraps around upon reaching the end
    private void AdvanceToNextTrack()
    {
        // Increments index and wraps to 0 when surpassing array boundary
        currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;

        PlayCurrentTrack();
    }

    // Loads current clip into AudioSource and initiates audio playback
    private void PlayCurrentTrack()
    {
        if (playlist[currentTrackIndex] != null)
        {
            audioSource.clip = playlist[currentTrackIndex];
            audioSource.Play();
            Debug.Log("Now playing track " + (currentTrackIndex + 1) + " of " + playlist.Length + ": " + playlist[currentTrackIndex].name);
        }
    }
}