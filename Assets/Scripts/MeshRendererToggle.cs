
using UnityEngine;
using System.Collections;

public class MeshRendererToggle : MonoBehaviour
{
    [Header("Mesh Renderer Settings")]
    public Renderer targetRenderer;

    [Header("Visibility Settings")]
    public bool startVisible = true;

    [Header("Random Timer Settings")]
    public bool useTimer = true;
    public float minTime = 10f;
    public float maxTime = 20f;

    private bool isVisible;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        isVisible = startVisible;
        ApplyVisibility();

        if (useTimer)
            StartCoroutine(ToggleTimer());
    }

    IEnumerator ToggleTimer()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            isVisible = !isVisible;
            ApplyVisibility();
        }
    }

    void ApplyVisibility()
    {
        if (targetRenderer != null)
            targetRenderer.enabled = isVisible;
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        ApplyVisibility();
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        ApplyVisibility();
    }
}
