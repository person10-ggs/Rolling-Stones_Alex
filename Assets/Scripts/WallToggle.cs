using UnityEngine;

public class WallToggle : MonoBehaviour
{
    public float switchTime = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SwitchWall), switchTime, switchTime);
    }

    private void SwitchWall()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}