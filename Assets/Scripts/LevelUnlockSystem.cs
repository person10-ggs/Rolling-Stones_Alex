
using UnityEngine;

public class LevelUnlockSystem : MonoBehaviour
{
    [System.Serializable]
    public class LevelPortal
    {
        public GameObject portal;
        public int levelNumber;
    }

    public LevelPortal[] levelPortals;

    private const string SaveKey = "HighestUnlockedLevel";

    private int HighestUnlockedLevel
    {
        get { return PlayerPrefs.GetInt(SaveKey, 1); }
    }

    private void Start()
    {
        UpdatePortals();
    }

    public void UnlockLevel(int levelNumber)
    {
        if (levelNumber > HighestUnlockedLevel)
        {
            PlayerPrefs.SetInt(SaveKey, levelNumber);
            PlayerPrefs.Save();
        }

        UpdatePortals();
    }

    public void UpdatePortals()
    {
        foreach (LevelPortal level in levelPortals)
        {
            if (level.portal != null)
            {
                level.portal.SetActive(
                    level.levelNumber <= HighestUnlockedLevel
                );
            }
        }
    }
}
